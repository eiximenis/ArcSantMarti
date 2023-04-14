using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Tests.Extensions
{
    static class Z80RegistersExtensions
    {
        public static byte GetByteRegisterByName(this Z80Registers regs, string name) => name switch
        {
            "B" => regs.Main.B,
            "C" => regs.Main.C,
            "D" => regs.Main.D,
            "E" => regs.Main.E,
            "H" => regs.Main.H,
            "L" => regs.Main.L,
            "A" => regs.Main.A,
            "F" => (byte)regs.Main.F,
            _ => 0x0
        };
        
        public static ushort GetWordRegisterByName(this Z80Registers regs, string name) => name switch
        {
            "BC" => regs.Main.BC,
            "DE" => regs.Main.DE,
            "HL" => regs.Main.HL,
            "AF" => regs.Main.AF,
            _ => 0x0
        };

        public static void SetByteRegisterByName(this Z80Registers regs, string name, byte value)
        {
            switch (name)
            {
                case "B": regs.Main.B = value; break;
                case "C": regs.Main.C = value; break;
                case "D": regs.Main.D = value; break;
                case "E": regs.Main.E = value; break;
                case "H": regs.Main.H = value; break;    
                case "L": regs.Main.L = value; break;
                case "A": regs.Main.A = value; break;
                case "F": regs.Main.F = (Z80Flags)value; break;
            }
        }
        
        public static void SetWordRegisterByName(this Z80Registers regs, string name, ushort value)
        {
            switch (name)
            {
                case "BC": regs.Main.BC = value; break;
                case "DE": regs.Main.DE = value; break;
                case "HL": regs.Main.HL = value; break;
                case "AF": regs.Main.AF = value; break;
            }
        }
    }
}
