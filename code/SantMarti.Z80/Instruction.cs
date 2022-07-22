using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80
{


    // Action parameters: Instruction itself, Processor, current PC address, (ret) new PC address
    public record Instruction(byte Opcode, string Name, int TStates, Func<Instruction, Z80Processor, ushort, ushort>? Action = null)   
    {
        public static Instruction Nop(byte opc) => new Instruction(opc, "NOP", 4);
    }

}
