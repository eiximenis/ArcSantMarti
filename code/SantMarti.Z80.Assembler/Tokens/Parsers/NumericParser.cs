using System.Globalization;
using System.Text.RegularExpressions;
using SantMarti.Z80.Assembler.Encoders;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;

// A number is:
// - A byte literal in decimal form (0-255)
// - A byte literal in hexadecimal form ($00-$FF)
// - A word literal in hexadecimal form ($0000-$FFFF)
// - A word literal in decimal form (0-65535)


public class NumericParser
{
    private const string regexString = @"^(\$?[0-9a-fA-F]{1,5})";
    
    private static Regex _regex = new Regex(regexString, RegexOptions.Compiled);
    
    public static TokenParseResult<NumericValue> TryGetNumber(string operand)
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
                var parsedValue =  new NumericValue(operand, value, value <= 255);
                return TokenParseResult<NumericValue>.Success(parsedValue);
            }
        }

        return TokenParseResult<NumericValue>.Error($"Value '{operand}' can't be parsed as a NumericValue");
    }

}