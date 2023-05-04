using System.Runtime.CompilerServices;

namespace SantMarti.Z80;


[Flags]
public enum OtherPins : ushort
{
    NONE = 0x0,
    MREQ = 0x1,
    IORQ = 0x2,
    RD = 0x4,
    WR = 0x8,
    M1 = 0x10,
    WAIT = 0x20,
    INT = 0x40,
    RESET= 0x80,
    RFSH = 0x100,
    HALT = 0x200,
    // Standard Combinations
    MEMORY_READ = MREQ | RD,
    MEMORY_WRITE = MREQ | WR,
    IO_READ = IORQ | RD,
    IO_WRITE = IORQ | WR
}

/// <summary>
/// This struct represents the pins of the Z80 processor
/// Info extracted from https://floooh.github.io/2017/12/10/z80-emu-evolution.html
/// </summary>
public struct Z80Pins
{
    // A0..A15: this is the 16 bit address-bus, used for addressing 64 KBytes of memory or as port number for
    // communicating with other chips and hardware devices
    public ushort Address;
    // D0..D7: this is the 8 bit data-bus, the address bus says ‘where’ to read or write something,
    // and the data bus says ‘what’
    public byte Data;
    // Other pins encoded as bits. Currenty only those will be implemented:
    // 0 -> MREQ (out): the ‘memory request’ pin is active when the CPU wants to perform a memory access
    // 1 -> IORQ (out): the ‘I/O request’ pin is active when the CPU wants to perform an I/O access
    // 2 -> RD (out): the ‘read’ pin is used together with the MREQ or IORQ to identify a memory-read or IO-device-input operation
    // 3 -> WR (out): the ‘write’ pin is used together with the MREQ or IORQ to identify a memory-write or IO-device-output operation
    // 4 -> M1 (out): ‘machine cycle one’, this pin is active during an opcode fetch machine cycle and can be used to differentiate an opcode fetch from a normal memory read operation
    // 5 -> WAIT (in): this pin is set to active by the ‘system’ to inject a wait state into the CPU, the CPU will only check this pin during a read or write operation
    // 6 -> INT (in): this pin is set to active by the ‘system’ to initiate an interrupt- request cycle
    // 7 -> RESET(in): this pin is set to active by the ‘system’ to reset the CPU
    
    public ushort Others;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReplaceOtherPinsWith(OtherPins pins) => Others = (ushort)pins;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ActivateOtherPins(OtherPins pins) => Others |= (ushort)pins;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DeactivateOtherPins(OtherPins pins) => Others &= (ushort)~pins;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool OthersAreSet(OtherPins pins) => (Others & (ushort)pins) == (ushort)pins;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Halted() => (Others & (ushort)OtherPins.HALT) != 0;
}