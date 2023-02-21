using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;

namespace SantMarti.Z80.Assembler.Builders
{
    class AssemblerLineBuilder
    {
        private readonly Dictionary<string, Func<TokenizedLine, AssemblerLineResult>> _builders;
        private readonly AssemblerLineParser _parser;

        public AssemblerLineBuilder()
        {
            _builders = new Dictionary<string, Func<TokenizedLine, AssemblerLineResult>>();
            _builders.Add("LD", LDBuilder.BuildFromLine);
            _builders.Add("EXX", l => AssemblerLineResult.Success(new [] { Z80Opcodes.EXX }));
            _builders.Add("ADD", ADDBuilder.BuildFromLine);
            _parser = new AssemblerLineParser();
        }
        
        public TokenizedLine Parse(string line)
        {
            return _parser.Parse(line);
        }

        public AssemblerLineResult Build(TokenizedLine line)
        {
            var opcode = line.GetOpcode();
            if (opcode is null)
            {
                return AssemblerLineResult.Error("No opcode found!", opcode);
            }
            
            if (_builders.TryGetValue(opcode.StrValue, out var builder))
            {
                return builder(line);
            }

            return AssemblerLineResult.Error($"Unknown opcode: {opcode.StrValue}", opcode);
        }
    }
}
