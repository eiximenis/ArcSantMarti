using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanMarti.Z80
{
    /// <summary>
    /// Implements the Z80 Registers.
    /// References: 
    /// - Z80 Architecture: http://www.z80.info/z80arki.htm
    /// - Z80 Registers: http://z80-heaven.wikidot.com/the-registers-and-memory
    /// </summary>
    public class Z80Registers
    {
        // 8 bit general purpose registers
        public byte A { get; set; }
        public byte B { get; set; }
        public byte C { get; set; }
        public byte D { get; set; }
        public byte E { get; set; }
        public byte F { get; set; }
        public byte H { get; set; }
        public byte L { get; set; }


        // 16 bit general purpose registers
        public ushort IX { get; set; }
        public ushort IY { get; set; }
        // 16 bit general purpose register pairs
        public ushort AF
        {
            get  => (ushort)(F + (A << 8));
        }
        public ushort BC
        {
            get => (ushort)(C + (B << 8));
        }
        public ushort DE
        {
            get => (ushort)(E + (D << 8));
        }

        public ushort HL
        {
            get => (ushort)(L + (H << 8));
        }
        // 16 bits specific registers
        public ushort PC { get; set; }          // Program Counter
        public ushort SP { get; set; }          // Stack Pointer


    }
}
