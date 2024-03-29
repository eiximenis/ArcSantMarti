namespace SantMarti.Z80.Tests;

public record TickData(Z80Pins pins);

/// <summary>
/// This class is used to test the Z80 in isolation
/// It offers a programable OnTick handler to test:
///     - Z80 pins (read/write, address, data, ...)
///     - Z80 opcodes behavior
///     - Simulate reads/writes to memory or generic in/out
/// </summary>
public class TestTickHandler
{
    private readonly Z80Processor _processor;
    private readonly List<(ushort Address, byte Data)> _memoryWrites = new();
    private readonly List<ushort> _memoryReads = new();
    private readonly Dictionary<ushort, Func<byte>> _memoryReaders = new();
    
    private int _writeTicks = 0;
    
    
    private Func<ushort, byte> _defaultMemoryReader = address => 0x0;
    
    public TestTickHandler OnDefaultMemoryRead(Func<ushort, byte> defaultReader)
    {
        _defaultMemoryReader = defaultReader;
        return this;
    }
    
    public TestTickHandler OnMemoryRead(ushort address, Func<byte> reader)
    {
        _memoryReaders.Add(address, reader);
        return this;
    }

    public TestTickHandler OnMemoryRead(ushort address, byte fixedValue)
    {
        _memoryReaders.Add(address, () => fixedValue);
        return this;
    }

    public TestTickHandler OnMemoryRead(ushort startAddress, IEnumerable<byte> data)
    {
        var address = startAddress;
        foreach (var databyte in data)
        {
            OnMemoryRead(address, () => databyte);
            address++;
        }
        
        return this;
    }
    
    private readonly List<TickData> _tickData = new();

    public TestTickHandler(Z80Processor processorUnderTest)
    {
        _processor = processorUnderTest;
        processorUnderTest.SetTickHandler(OnTick);
    }

    private void OnTick(ref Z80Pins pins, int ticks)
    {
        for (int tdata = 0; tdata < ticks; tdata++)
        {
            _tickData.Add(new TickData(pins));
        }

        if (pins.OthersAreSet(OtherPins.MEMORY_READ))
        {
            DoReadMemory(ref pins, ticks);
        }
        else if (pins.OthersAreSet(OtherPins.MEMORY_WRITE))
        {
            _memoryWrites.Add((pins.Address, pins.Data));
        }
    }
    
    private void DoReadMemory(ref Z80Pins pins, int ticks)
    {
        _writeTicks += ticks;
        if (_writeTicks < 2) return;            // We are slow writer, and we need TWO ticks
        _writeTicks = 0;
        _memoryReads.Add(pins.Address);
        if (_memoryReaders.ContainsKey(pins.Address))
        {
            pins.Data = _memoryReaders[pins.Address]();
        }
        else
        {
            pins.Data = _defaultMemoryReader(pins.Address);
        }
    }
    
    public int TotalTicks => _tickData.Count;
    public IEnumerable<(ushort Address, byte Data)> MemoryWrites => _memoryWrites;
    public bool HasAddressBeenRead(ushort address) => _memoryReads.Contains(address);
    public bool HasAddressBeenWritten(ushort address) => _memoryWrites.Any(w => w.Address == address);
    public IEnumerable<ushort> MemoryReads => _memoryReads;
    public int TotalMemoryReads => _memoryReads.Count;

    /// <summary>
    /// Returns the byte written to memory at the given address if any
    /// If memory has not written to the given address, returns -1 
    /// </summary>
    public short GetMemoryWrittenAt(ushort address)
    {
        foreach (var (addr, data) in _memoryWrites)
        {
            if (addr == address)
            {
                return data;
            }
        }
        return -1;
    }

    public void ResetTicks() => _tickData.Clear();
}