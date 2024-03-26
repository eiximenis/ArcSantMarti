using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Extensions;

namespace SantMarti.Z80.Instructions
{
    public static class Load
    {

        /// <summary>
        /// LD R,R2: Loads R2 into R
        /// </summary>
        public static void LD_R_R (Instruction instruction, Z80Processor processor)
        {
            var opcode = instruction.Opcode;
            var source = opcode & 0b00_000_111;
            var dest = (opcode & 0b00_111_000) >> 3;
            processor.SetByteRegisterByMask(dest, processor.GetByteRegisterMask(source));
        }

        /// <summary>
        /// LD R, n: Loads n (byte) into R
        /// </summary>
        public static void LD_R_N (Instruction instruction, Z80Processor processor)
        {
            var opcode = instruction.Opcode;
            var target = (opcode & 0b00_111_000) >> 3;
            var source = processor.MemoryRead();
            processor.SetByteRegisterByMask(target, source);
        }

        /// <summary>
        /// LD A, (nn): Loads memory address *nn into A 
        /// </summary>
        public static void LD_A_NN(Instruction instruction, Z80Processor processor)
        {
            processor.Registers.Z = processor.MemoryRead();
            processor.Registers.W = processor.MemoryRead();
            var address = processor.Registers.WZ;
            processor.Registers.Main.A = processor.MemoryRead(address);
            processor.Registers.WZ = (ushort)(address + 1);         // MEMPTR behavior
        }
        
        /// <summary>
        /// LD (HL), R: Loads R into *HL
        /// </summary>
        public static void LD_HLRef_R(Instruction instruction, Z80Processor processor)
        {
            var opcode = instruction.Opcode;
            var source = opcode & 0b00_000_111;
            var value = processor.GetByteRegisterMask(source);
            processor.MemoryWrite(processor.Registers.Main.HL, value);
        }
        
        // LD R, (HL): Loads *HL into R 
        public static void LD_R_HLRef(Instruction instruction, Z80Processor processor)
        {
            var opcode = instruction.Opcode;
            var target = (opcode & 000_111_000) >> 3;
            var data = processor.MemoryRead(processor.Registers.Main.HL);
            processor.SetByteRegisterByMask(target, data);
        }

        // LD A, (HL): Loads *HL into A
        // Cant use LD_R_HLRef because opcode does not follow the mask (000_111_000) for A
        public static void LD_A_HLRef(Instruction instruction, Z80Processor processor)
        {
            var data = processor.MemoryRead(processor.Registers.Main.HL);
            processor.Registers.Main.A = data;
        }
    }
}
