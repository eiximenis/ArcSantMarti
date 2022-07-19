using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SanMarti.Z80.Extensions;

namespace SanMarti.Z80.Instructions
{
    public static class LD
    {

        /// <summary>
        /// LD X,Y: Loads Y into D
        /// http://www.z80.info/z80syntx.htm#LD
        /// </summary>
        public static void LDXY (Instruction instruction, Z80Processor processor)
        {
            var opcode = instruction.Opcode;
            var source = opcode & 0b00_000_111;
            var dest = opcode & 0b00_111_000;
            processor.SetByteRegister(dest, processor.GetByteRegister(source));
        }
    }
}
