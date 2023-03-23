using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80
{


    // Action parameters: Instruction itself, Processor, current PC address, (ret) new PC address
    public record Instruction(byte Opcode, string Name, int TStates,  int TStates2, Action<Instruction, Z80Processor>? Action = null)   
    {
        public Instruction(byte opcode, string name, int tstates,  Action<Instruction, Z80Processor>? Action = null) : this(opcode, name, tstates, tstates, Action) { }
        public static Instruction Nop(byte opc) => new Instruction(opc, "NOP", 4);
    }

}
