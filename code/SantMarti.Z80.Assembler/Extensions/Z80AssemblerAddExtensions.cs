using SantMarti.Z80.Assembler.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;

namespace SantMarti.Z80.Assembler.Extensions
{
    public static class Z80AssemblerAddExtensions
    {
        public static void RADD_AN(this Z80AssemblerBuilder asmBuilder, byte value)
        {
            var numericValue = new NumericValue(value.ToString(), value, isByte:true);
            asmBuilder.Raw(ADDBuilder.ADD_A_N(numericValue).Bytes!);
        }

        public static void ADD_AHL(this Z80AssemblerBuilder asmBuilder) => asmBuilder.Raw(ADDBuilder.ADD_A_HLRef().Bytes!);

    }
}
