using SantMarti.Z80.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Instructions
{
    public static class Add
    {
        /// <summary>
        /// ADD A,n: Adds n to A
        /// </summary>
        public static ushort AddAN(Instruction instruction, Z80Processor processor, ushort pc)
        {
            var data = processor.FetchAt((ushort)(pc + 1));
            ref var registers = ref processor.Registers.Main;
            DoByteAdd(ref registers, data);
            return (ushort)(pc + 2);
        }
        
        /// <summary>
        ///  ADC A,n: Adds n + carry flag to A
        /// </summary>
        public static ushort AdcAN(Instruction instruction, Z80Processor processor, ushort pc)
        {
            var data = processor.FetchAt((ushort)(pc + 1));
            ref var registers = ref processor.Registers.Main;
            var carry = registers.HasFlag(Z80Flags.C) ? (byte)1 : (byte)0;
            DoByteAdd(ref registers, data, carry);
            return (ushort)(pc + 2);
        }



        /// <summary>
        /// ADD A,r: Adds value of register r to A
        /// </summary>
        public static void AddAR(Instruction instruction, Z80Processor processor)
        {
            ref var registers = ref processor.Registers.Main;
            // Add A,r opcode is 10000RRR where RRR is the register to add
            var value = processor.GetByteRegister(instruction.Opcode & 0b00000111);
            DoByteAdd(ref registers, value);
        }
        private static void DoByteAdd(ref Z80GenericRegisters registers, byte data, byte cflag = 0)
        {
            int result = registers.A + data + cflag ;
            int no_carry_sum = registers.A ^ (data + cflag);
            int carry_into = result ^ no_carry_sum;
            int half_carry = carry_into & 0x10;
            int carry = carry_into & 0x100;
            var byteResult = (byte)result;
            // For addition, operands with different signs never cause overflow. When adding operands
            // with similar signs and the result contains a different sign, the Overflow Flag is set
            var overflow = ((no_carry_sum & 0x80) == 0) && (((byteResult & 0x80) ^ (registers.A & 0x80)) != 0);
            registers.A = byteResult;
            registers.ClearFlag(Z80Flags.N);
            registers.SetFlagIf(Z80Flags.Z, byteResult == 0);
            registers.SetFlagIf(Z80Flags.H, half_carry);
            registers.SetFlagIf(Z80Flags.C, carry);
            registers.SetFlagIf(Z80Flags.PV, overflow);
            registers.SetFlagIf(Z80Flags.S, byteResult & 0x80);
            registers.CopyF3F5FlagsFrom(byteResult);
        }
    }
}
