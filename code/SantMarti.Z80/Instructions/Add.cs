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
        public static void AddAN(Instruction instruction, Z80Processor processor)
        {
            var data = processor.FetchNext();
            ref var registers = ref processor.Registers.Main;
            DoByteAdd(ref registers, data);
        }

        private static void DoByteAdd(ref Z80GenericRegisters registers, byte data)
        {
            int result = registers.A + data;
            int no_carry_sum = registers.A ^ data;
            int carry_into = result ^ no_carry_sum;
            int half_carry = carry_into & 0x10;
            int carry = carry_into & 0x100;
            var byteResult = (byte)result;
            // For addition, operands with different signs never cause overflow. When adding operands
            // with similar signs and the result contains a different sign, the Overflow Flag is set
            var overflow = (((registers.A & 0x80) ^ (data & 0x80)) == 0) && (((byteResult & 0x80) ^ (data & 0x80)) != 0);
            registers.A = byteResult;
            registers.ClearFlag(Z80Flags.N);
            registers.SetFlagIf(Z80Flags.Z, byteResult == 0);
            registers.SetFlagIf(Z80Flags.H, half_carry);
            registers.SetFlagIf(Z80Flags.C, carry);
            registers.SetFlagIf(Z80Flags.PV, overflow);
        }
    }
}
