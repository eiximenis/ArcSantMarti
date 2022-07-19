using SantMarti.Z80.Assembler.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler
{
    public class Z80AssemblerBuilder
    {
        private readonly List<byte> _bytes;
        private readonly AssemblerLineBuilder _lineBuilder;
        public Z80AssemblerBuilder()
        {
            _bytes = new List<byte>(1024);
            _lineBuilder = new AssemblerLineBuilder();
        }
        public IEnumerable<byte> Build() => _bytes;

        public void Asm(string line)
        {
            var bytes = _lineBuilder.Build(line);
            if (bytes is null)
            {
                throw new InvalidOperationException($"Can't parse line: {line}");
            }
            _bytes.AddRange(bytes);
        }

        public void LD(string dest, string source)
        {
            var builder = new LDBuilder(dest, source);
            _bytes.AddRange(builder.Build());
        }

        public void EXX()
        {
            _bytes.Add(Z80Opcodes.EXX);
        }

        public void ADDAN(byte value)
        {
            _bytes.Add(Z80Opcodes.ADD_AN);
            _bytes.Add(value);
        }

    }
}
