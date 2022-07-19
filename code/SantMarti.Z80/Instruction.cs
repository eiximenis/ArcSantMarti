using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanMarti.Z80
{
    public record Instruction(byte Opcode, string Name, int TStates, Action<Instruction, Z80Processor>? action = null)
    {
        public static Instruction Nop(byte opc) => new Instruction(opc, "NOP", 4);
    }

}
