using System.Text.RegularExpressions;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public class OpcodeParser
{
        private const string regexStringOpcode = @"^([A-Za-z_][A-Za-z0-9_]*)$";
        private static Regex _regexLabel = new Regex(regexStringOpcode, RegexOptions.Compiled);
    
        public static TokenParseResult<Opcode> TryGetLabel(string operand)
        {
            var match = _regexLabel.Match(operand);
            if (match.Success)
            {
                var name = match.Groups[1].Value;
                var parsedValue = new Opcode(name);
                return TokenParseResult<Opcode>.Success(parsedValue);
            }
            return TokenParseResult<Opcode>.Error($"Value '{operand}' can't be parsed as a Label");
        } 

}