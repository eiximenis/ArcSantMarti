using SantMarti.Z80.Assembler.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Extensions
{
    public static class Z80AssemblerAddExtensions
    {
        public static void ADD_AN(this Z80AssemblerBuilder asmBuilder, byte value)
        {
            asmBuilder.Raw(ADDBuilder.ADD_AN(value));
        }

        public static void ADD_AHL(this Z80AssemblerBuilder asmBuilder) => asmBuilder.Raw(ADDBuilder.ADD_AHL());

    }
}
