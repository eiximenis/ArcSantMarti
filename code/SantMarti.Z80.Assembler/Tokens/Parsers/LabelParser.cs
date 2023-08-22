using System.Text.RegularExpressions;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public class LabelParser
{
    private const string regexStringLabel = @"^([A-Za-z_][A-Za-z0-9_]*):$";

    private static Regex _regexLabel = new Regex(regexStringLabel, RegexOptions.Compiled);

    public static TokenParseResult<AssemblyLabel> TryGetLabel(string operand)
    {
        var match = _regexLabel.Match(operand);
        if (match.Success)
        {
            var name = match.Groups[1].Value;
            var parsedValue = new AssemblyLabel(name);
            return TokenParseResult<AssemblyLabel>.Success(parsedValue);
        }
        
        return TokenParseResult<AssemblyLabel>.Error($"Value '{operand}' can't be parsed as a Label");
    } 

}