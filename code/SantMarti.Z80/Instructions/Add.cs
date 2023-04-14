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
        public static void AddAN(Instruction instruction, Z80Processor processor)
        {
            var data = processor.MemoryRead();
            ref var registers = ref processor.Registers.Main;
            processor.Registers.Main.A = Z80Alu.ByteAdd(ref registers, processor.Registers.Main.A, data);
        }
        
        /// <summary>
        ///  ADC A,n: Adds n + carry flag to A
        /// </summary>
        public static void AdcAN(Instruction instruction, Z80Processor processor)
        {
            var data = processor.MemoryRead();
            ref var registers = ref processor.Registers.Main;
            var carry = registers.HasFlag(Z80Flags.Carry) ? (byte)1 : (byte)0;
            processor.Registers.Main.A = Z80Alu.ByteAdd(ref registers, data, carry);
        }

        /// <summary>
        /// ADD A,r: Adds value of register r to A
        /// </summary>
        public static void AddAR(Instruction instruction, Z80Processor processor)
        {
            ref var registers = ref processor.Registers.Main;
            // Add A,r opcode is 10000RRR where RRR is the register to add
            var value = processor.GetByteRegister(instruction.Opcode & 0b00000111);
            processor.Registers.Main.A = Z80Alu.ByteAdd(ref registers, processor.Registers.Main.A, value);
        }
    }
}
