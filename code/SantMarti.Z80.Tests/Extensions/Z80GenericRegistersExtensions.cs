using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Tests.Extensions
{
    static class Z80GenericRegistersExtensions
    {
        public static byte GetByteRegisterByName(in this Z80GenericRegisters regs, string name) => name switch
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
        
        public static ushort GetWordRegisterByName(in this Z80GenericRegisters regs, string name) => name switch
        {
            "BC" => regs.BC,
            "DE" => regs.DE,
            "HL" => regs.HL,
            "AF" => regs.AF,
            _ => 0x0
        };

        public static void SetByteRegisterByName(ref this Z80GenericRegisters regs, string name, byte value)
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
            }
        }
        
        public static void SetWordRegisterByName(ref this Z80GenericRegisters regs, string name, ushort value)
        {
            switch (name)
            {
                case "BC": regs.BC = value; break;
                case "DE": regs.DE = value; break;
                case "HL": regs.HL = value; break;
                case "AF": regs.AF = value; break;
            }
        }
    }
}
