using SantMarti.Z80;

namespace SantMarti.Spectrum.Memory;

public class SpectrumMemory
{

    
    private readonly byte[] _data;
    public int Size { get; }
    public int RomOffset { get; }
    public int RomLength { get; }
    public int RamLength { get; }
    
    public int ScreenMemoryOffset { get; init; }
    public int ScreenMemoryColorDataOffset { get; init; }
    public int PrinterBufferOffset { get; init; }
    public int SystemVariables { get; init; }
    public int ReservedOffset { get; init; }
    public int AvailableRamOffset { get; init; }
    public int Reserved2Offset { get; init; }
    
    public Span<byte> Data => _data;

    public SpectrumMemory(int romKKib, int ramKib)
    {
        RamLength = ramKib * 1024;
        RomLength = romKKib * 1024;
        _data = new byte[RomLength + RamLength];
    }



    public async Task LoadRomFromFile(string filePath)
    {
        var romFileInfo = new FileInfo(filePath);
        var romFileSize= romFileInfo.Length;
        if (romFileSize != RomLength)
        {
            throw new InvalidOperationException($"File of {romFileSize} bytes can't be loaded on ROM of {RomLength} bytes" );
        }

        await using var fs = File.OpenRead(filePath);
        var destination = new MemoryStream(_data, 0, RomLength);
        await fs.CopyToAsync(destination);
        fs.Close();
    }


}