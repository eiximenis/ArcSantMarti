using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Extensions
{
    public static class Z80ProcessorExtensions
    {
        public static byte GetByteRegisterMask(this Z80Processor processor, int regId)
        {
            ref var registers = ref processor.Registers.Main;
            return regId switch
            {
                0b000 => registers.B,
                0b001 => registers.C,
                0b010 => registers.D,
                0b011 => registers.E,
                0b100 => registers.H,
                0b101 => registers.L,
                0b111 => registers.A,
                _ => 0x00
            };
        }

        public static void SetByteRegisterByMask(this Z80Processor processor, int regId, byte value)
        {
            ref var registers = ref processor.Registers.Main;

            switch (regId)
            {
                case 0b000: registers.B = value; break;
                case 0b001: registers.C = value; break;
                case 0b010: registers.D = value; break;
                case 0b011: registers.E = value; break;
                case 0b100: registers.H = value; break;
                case 0b101: registers.L = value; break;
                case 0b111: registers.A = value; break;
                default: break;
            }
        }

        public static ushort GetWordRegisterMask(this Z80Processor processor, int regid)
        {
            ref var registers = ref processor.Registers.Main;
            return regid switch
            {
                0b00 => registers.BC,
                0b01 => registers.DE,
                0b10 => registers.HL,
                0b11 => processor.Registers.SP,
                _ => 0x0000
            };
        }
    }
}
