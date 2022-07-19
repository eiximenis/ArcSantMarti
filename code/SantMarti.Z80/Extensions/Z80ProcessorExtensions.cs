using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanMarti.Z80.Extensions
{
    public static class Z80ProcessorExtensions
    {
        public static byte GetByteRegister(this Z80Processor processor, int regId)
        {
            return regId switch
            {
                0b000 => processor.Registers.B,
                0b001 => processor.Registers.C,
                0b010 => processor.Registers.D,
                0b011 => processor.Registers.E,
                0b100 => processor.Registers.H,
                0b101 => processor.Registers.L,
                0b111 => processor.Registers.A,
                _ => 0x00
            };
        }

        public static void SetByteRegister(this Z80Processor processor, int regId, byte value)
        {
            switch (regId)
            {
                case 0b000: processor.Registers.B = value; break;
                case 0b001: processor.Registers.C = value; break;
                case 0b010: processor.Registers.D = value; break;
                case 0b011: processor.Registers.E = value; break;
                case 0b100: processor.Registers.H = value; break;
                case 0b101: processor.Registers.L = value; break;
                case 0b111: processor.Registers.A = value; break;
                default: break;
            }
        }
    }
}
