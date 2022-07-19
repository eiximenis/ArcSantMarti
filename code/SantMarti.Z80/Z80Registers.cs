using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80
{
    /// <summary>
    /// Flags of the Z80
    /// http://www.z80.info/z80sflag.htm
    /// </summary>
    [Flags]
    public enum Z80Flags : byte
    {
        C = 1 << 0,     // Carry: Set if the result did not fit in the register
        N = 1 << 1,     // Subtract: Set if the last operation was a subtraction
        PV = 1 << 2,    // Parity or Overflow: Parity set if even number of bits set / Overflow set if the 2-complement result does not fit in the register
        F3 = 1 << 3,    // Undocumented (Copy of bit 3)
        H = 1 << 4,     // Half Carry: Carry from bit 3 to bit 4
        F5 = 1 << 5,    // Undocumented (Copy of bit 5)
        Z =  1 << 6,    // Set if the value is zero
        S = 1 << 7      // Set if the 2-complement value is negative (copy of MSB)
    }


    /// <summary>
    /// Implements the Z80 Registers.
    /// References: 
    /// - Z80 Architecture: http://www.z80.info/z80arki.htm
    /// - Z80 Registers: http://z80-heaven.wikidot.com/the-registers-and-memory
    /// </summary>
    /// 
    [StructLayout(LayoutKind.Explicit)]
    public struct Z80GenericRegisters
    {
        // Byte B,C and Word BC
        [FieldOffset(0)]
        public byte C;
        [FieldOffset(1)]
        public byte B;
        [FieldOffset(0)]
        public ushort BC;
        // Byte D,E and Word DE
        [FieldOffset(4)]
        public byte E;
        [FieldOffset(5)]
        public byte D;
        [FieldOffset(4)]
        public ushort DE;
        // Byte H,L and Word HL
        [FieldOffset(8)]
        public byte L;
        [FieldOffset(9)]
        public byte H;
        [FieldOffset(8)]
        public ushort HL;
        // Flags, Accumulator and Word AF
        [FieldOffset(12)]
        public Z80Flags F;
        [FieldOffset(13)]
        public byte A;
        [FieldOffset(12)]
        public short AF;
    }

    public class Z80Registers {

        private Z80GenericRegisters _main;
        private Z80GenericRegisters _alternate;

        // Generic registers (B,C,D,E,H,L,A,F)
        public ref Z80GenericRegisters  Main {get => ref _main; }
        // Generic alternate registers (B',C',D',E',H',L', A',F')
        public ref Z80GenericRegisters Alternate { get => ref _alternate; }

        // 16 bit general purpose registers
        public ushort IX { get; set; }
        public ushort IY { get; set; }

        
        // 16 bits specific registers
        public ushort PC { get; set; }          // Program Counter
        public ushort SP { get; set; }          // Stack Pointer

    }
}
