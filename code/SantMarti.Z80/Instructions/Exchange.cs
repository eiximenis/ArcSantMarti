using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Instructions
{
    public static class Exchange
    {
        public static void EXAFAF2(Instruction instruction, Z80Processor processor)
        {
            ref var main = ref processor.Registers.Main;
            ref var alternate = ref processor.Registers.Alternate;            
            (alternate.AF, main.AF) = (main.AF, alternate.AF);
        }

        public static void EXDEHL(Instruction instruction, Z80Processor processor)
        {
            ref var registers = ref processor.Registers.Main;
            (registers.DE, registers.HL) = (registers.HL, registers.DE);
        }

        /// <summary>
        /// EXX: EXCHANGE ALTERNATE REGISTERS
        /// http://www.z80.info/z80syntx.htm#EXX
        /// </summary>
        public static void EXX(Instruction instruction, Z80Processor processor)
        {
            ref var main = ref processor.Registers.Main;
            ref var alternate = ref processor.Registers.Alternate;

            (main.BC, alternate.BC) = (alternate.BC, main.BC);
            (main.DE, alternate.DE) = (alternate.DE, main.DE);
            (main.HL, alternate.HL) = (alternate.HL, main.HL);
        }
    }
}
