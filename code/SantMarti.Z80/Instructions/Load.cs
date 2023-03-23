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
        /// LD X,Y: Loads Y into D
        /// http://www.z80.info/z80syntx.htm#LD
        /// </summary>
        public static void LD_R_R (Instruction instruction, Z80Processor processor)
        {
            var opcode = instruction.Opcode;
            var source = opcode & 0b00_000_111;
            var dest = (opcode & 0b00_111_000) >> 3;
            processor.SetByteRegister(dest, processor.GetByteRegister(source));
        }
        
        public static void LD_A_NN(Instruction instruction, Z80Processor processor)
        {
            var lobyte = processor.ReadMemoryAddress((ushort)(processor.Registers.PC + 1));
            var hibite = processor.ReadMemoryAddress((ushort)(processor.Registers.PC + 2));
            var address = (ushort)((hibite << 8) | lobyte);
            processor.Registers.Main.A = processor.ReadMemoryAddress(address);
        }
    }
}
