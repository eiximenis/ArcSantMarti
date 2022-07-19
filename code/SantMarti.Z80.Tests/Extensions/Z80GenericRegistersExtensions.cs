using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Tests.Extensions
{
    static class Z80GenericRegistersExtensions
    {
        public static byte GetByName(in this Z80GenericRegisters regs, string name) => name switch
        {
            "B" => regs.B,
            "C" => regs.C,
            "D" => regs.D,
            "E" => regs.E,
            "H" => regs.H,
            "L" => regs.L,
            "A" => regs.A,
            "F" => (byte)regs.F,
            _ => 0x0
        };

        public static void SetByName(ref this Z80GenericRegisters regs, string name, byte value)
        {
            switch (name)
            {
                case "B": regs.B = value; break;
                case "C": regs.C = value; break;
                case "D": regs.D = value; break;
                case "E": regs.E = value; break;
                case "H": regs.H = value; break;    
                case "L": regs.L = value; break;
                case "A": regs.A = value; break;
                case "F": regs.F = (Z80Flags)value; break;
                default: break;
            }
        }
    }
}
