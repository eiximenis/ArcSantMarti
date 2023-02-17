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

        public void Raw(IEnumerable<byte> bytes)
        {
            _bytes.AddRange(bytes);
        }

        public IEnumerable<byte> Build() => _bytes;

        public IEnumerable<byte> Asm(string line)
        {
            var bytes = _lineBuilder.Build(line);
            if (bytes is null)
            {
                throw new InvalidOperationException($"Can't parse line: {line}");
            }
            _bytes.AddRange(bytes);
            return bytes;
        }

        public void EXX()
        {
            _bytes.Add(Z80Opcodes.EXX);
        }


        public void ADD(string source, string target)
        {
            _bytes.AddRange(ADDBuilder.ADD(source, target));
        }

        public void LD(string dest, string source)
        {
            _bytes.AddRange(LDBuilder.LD(dest, source));
        }





    }
}
