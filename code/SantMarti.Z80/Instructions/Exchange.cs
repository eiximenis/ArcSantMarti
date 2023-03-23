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
            var temp = alternate.AF;
            alternate.AF = main.AF;
            main.AF = temp;
        }

        public static void EXDEHL(Instruction instruction, Z80Processor processor)
        {
            ref var registers = ref processor.Registers.Main;
            var temp = registers.HL;
            registers.HL = registers.DE;
            registers.DE = temp;
        }

        /// <summary>
        /// EXX: EXCHANGE ALTERNATE REGISTERS
        /// http://www.z80.info/z80syntx.htm#EXX
        /// </summary>
        public static void EXX(Instruction instruction, Z80Processor processor)
        {
            ref var main = ref processor.Registers.Main;
            ref var alternate = ref processor.Registers.Alternate;

            var temp = alternate.BC;
            alternate.BC = main.BC;
            main.BC = temp;

            temp = alternate.DE;
            alternate.DE = main.DE;
            main.DE = temp;

            temp = alternate.HL;
            alternate.HL = main.HL;
            main.HL = temp;
        }
    }
}
