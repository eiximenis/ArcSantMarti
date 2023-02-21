using System.Globalization;
using System.Text.RegularExpressions;
using SantMarti.Z80.Assembler.Encoders;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;

// A number is:
// - A byte literal in decimal form (0-255)
// - A byte literal in hexadecimal form ($00-$FF)
// - A word literal in hexadecimal form ($0000-$FFFF)
// - A word literal in decimal form (0-65535)


public class MemoryReferenceParser
{
    private const string regexString = @"^\(\s*(\$?[0-9a-fA-F]{1,5})\s*\)";
    
    private static Regex _regex;
    
    static MemoryReferenceParser()
    {
        _regex = new Regex(regexString, RegexOptions.Compiled);
    }

    public static TokenParseResult<MemoryReference> TryGetMemoryReference(string operand)
    {
        var match = _regex.Match(operand);
        if (match.Success)
        {
            var number = match.Groups[1].ValueSpan;
            var nbase = 10;
            if (number[0] == '$')
            {
                nbase = 16;
                number = number[1..];
            }

            if (int.TryParse(number,  nbase == 16 ? NumberStyles.HexNumber : NumberStyles.None, CultureInfo.InvariantCulture, out var value))
            {
                var parsedValue = new MemoryReference(operand, value);
                return TokenParseResult<MemoryReference>.Success(parsedValue);
            }
        }

        return TokenParseResult<MemoryReference>.Error($"Value '{operand}' can't be parsed as a MemoryReference");
    }

}