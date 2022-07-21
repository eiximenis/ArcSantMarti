using SantMarti.Z80.Assembler.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Extensions
{
    public static class Z80AssemblerLoadExtensions
    {
        public static void LD_RR(this Z80AssemblerBuilder asmBuilder, string source, string target)
        {
            asmBuilder.Raw(LDBuilder.LD_RR(source, target));
        }
    }
}
