using SantMarti.Z80.Assembler.Builders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;

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

        public AssemblerLineResult Asm(string line)
        {
            var tokenizedLine = _lineBuilder.Parse(line);

            var anyUnkwon = tokenizedLine.Tokens.FirstOrDefault(t => t is UnknownToken);
            if (anyUnkwon is not null)
            {
                return AssemblerLineResult.Error($"Unkonwn token found: {anyUnkwon.StrValue}");
            }

            var result = _lineBuilder.Build(tokenizedLine);

            if (result.HasResult)
            {
                _bytes.AddRange(result.Bytes!);
            }

            return result;
        }

        public TokenizedLine Tokenize(string line) => _lineBuilder.Parse(line);

        public void EXX()
        {
            _bytes.Add(Z80Opcodes.EXX);
        }

        public void NOP() => _bytes.Add(Z80Opcodes.NOP);

        public void HALT() => _bytes.Add(Z80Opcodes.HALT);
        public void ADD(string source, string target)
        {
            var result = ADDBuilder.ADD(source, target);
            ProcessParseResult(result);
        }

        public void LD(string dest, string source)
        {
            var result = LDBuilder.LD(dest, source);
            ProcessParseResult(result);
        }

        public void PUSH(string dest)
        {
            var result = PUSHBuilder.PUSH(dest);
            ProcessParseResult(result);
        }
        
        public void POP(string dest)
        {
            var result = POPBuilder.POP(dest);
            ProcessParseResult(result);
        }

        public void INC(string dest)
        {
            var result = INCBuilder.INC(dest);
            ProcessParseResult(result);
        }

        public void DEC(string dest)
        {
            var result = DECBuilder.DEC(dest);
            ProcessParseResult(result);
        }
        
        public void DAA() => _bytes.Add(Z80Opcodes.DAA);

        public void DJNZ(string operand)
        {
            var result = DJNZBuilder.JP(operand);
            ProcessParseResult(result);
            
        }
        public void JP(string operand)
        {
            var result = JPBuilder.JP(operand);
            ProcessParseResult(result);
        }

        public void JR(string operand)
        {
            var result = JRBuilder.JR(operand);
            ProcessParseResult(result);
        }
        
        public void AND(string operand)
        {
            var result = ANDBuilder.AND(operand);
            ProcessParseResult(result);
        }
        
        public void OR(string operand)
        {
            var result = ORBuilder.OR(operand);
            ProcessParseResult(result);
        }
        
        public void XOR(string operand)
        {
            var result = XORBuilder.XOR(operand);
            ProcessParseResult(result);
        }

        private void ProcessParseResult(AssemblerLineResult result)
        {
            if (result.HasResult)
            {
                _bytes.AddRange(result.Bytes!);
            }
            else
            {
                throw new InvalidOperationException($"Error parsing line: {result.ErrorMessage}");
            }
        }
        
    }
}
