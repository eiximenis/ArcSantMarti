using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Builders
{
    class AssemblerLineBuilder
    {
        private readonly Dictionary<string, Func<string, string, byte[]?>> _builders;

        public AssemblerLineBuilder()
        {
            _builders = new Dictionary<string, Func<string, string, byte[]?>>();
            _builders.Add("LD", LDBuilder.BuildFromLine);
            _builders.Add("EXX", (k, l) => new[] { Z80Opcodes.EXX });
            _builders.Add("ADD", ADDBuilder.BuildFromLine);
        }

        public byte[]? Build(string asm)
        {
            var semicolon = asm.IndexOf(';');
            if (semicolon != -1)
            {
                asm = asm.Substring(0, semicolon);
            }
            var trimmed = asm.ToUpperInvariant().Trim();
            var space = trimmed.IndexOf(' ');
            var keyword = space != -1 ? trimmed.Substring(0, space) : trimmed;
            var restLine = space != -1 ? trimmed.Substring(space + 1) : "";
            

            if (_builders.TryGetValue(keyword, out var builder))
            {
                return builder(keyword, restLine.Trim());
            }

            return null;
        }
    }
}
