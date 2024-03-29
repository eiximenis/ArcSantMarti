﻿using SantMarti.Z80.Extensions;
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
        public static void Add_N(Instruction instruction, Z80Processor processor)
        {
            var data = processor.MemoryRead();
            ref var registers = ref processor.Registers.Main;
            processor.Registers.Main.A = Z80Alu.Add8(ref registers, processor.Registers.Main.A, data);
        }

        /// <summary>
        ///  ADC A,n: Adds n + carry flag to A
        /// </summary>
        public static void Adc_N(Instruction instruction, Z80Processor processor)
        {
            var data = processor.MemoryRead();
            ref var registers = ref processor.Registers.Main;
            var carry = registers.HasFlag(Z80Flags.Carry) ? (byte)1 : (byte)0;
            processor.Registers.Main.A = Z80Alu.Add8(ref registers, processor.Registers.Main.A, data, carry);
        }

        /// <summary>
        /// ADD A,r: Adds value of register r to A
        /// </summary>
        public static void Add_R(Instruction instruction, Z80Processor processor)
        {
            ref var registers = ref processor.Registers.Main;
            // Add A,r opcode is 10000RRR where RRR is the register to add
            var value = processor.GetByteRegisterMask(instruction.Opcode & 0b00000111);
            registers.A = Z80Alu.Add8(ref registers, registers.A, value);
        }

        // ADD A,(HL): Adds *HL to A
        public static void Add_HLRef(Instruction instruction, Z80Processor processor)
        {
            ref var registers = ref processor.Registers.Main;
            var value = processor.MemoryRead(registers.HL);
            processor.Registers.Main.A = Z80Alu.Add8(ref registers, processor.Registers.Main.A, value);
        }

        // ADD HL, RR: Adds value of register pair RR to HL
        public static void Add_HL_RR(Instruction instruction, Z80Processor processor)
        {
            // Register Pair is encoded in the opcode as 00RRR1001
            var value = processor.GetWordRegisterMask(instruction.Opcode & 0b00110000);
            processor.Registers.Main.HL = Z80Alu.Add16(ref processor.Registers.Main, processor.Registers.Main.HL, value);

        }

        /// <summary>
        ///  SUB n: Substracts n from A
        /// </summary>
        public static void Sub_N(Instruction instruction, Z80Processor processor)
        {
            var data = processor.MemoryRead();
            ref var registers = ref processor.Registers.Main;
            processor.Registers.Main.A = Z80Alu.Sub8(ref registers, processor.Registers.Main.A, data);
        }

        /// <summary>
        /// SUB r: Substracts value of R from A
        /// </summary>
        public static void Sub_R(Instruction instruction, Z80Processor processor)
        {
            var operand = processor.GetByteRegisterMask(instruction.Opcode & 0b00000111);
            ref var registers = ref processor.Registers.Main;
            registers.A = Z80Alu.Sub8(ref registers, registers.A, operand, 0);
        }

        /// <summary>
        /// SUB (HL): Substracts *HL from A
        /// </summary>
        public static void Sub_HLRef(Instruction instruction, Z80Processor processor)
        {
            ref var registers = ref processor.Registers.Main;
            var value = processor.MemoryRead(registers.HL);
            processor.Registers.Main.A = Z80Alu.Sub8(ref registers, processor.Registers.Main.A, value);
        }
    }
}
