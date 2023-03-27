using System.Globalization;
using System.Text.RegularExpressions;
using SantMarti.Z80.Assembler.Encoders;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;


public enum NumericValueKind
{
    StandardValue,
    DisplacementOffset
}

// A number is:
// - A byte literal in decimal form (0-255)
// - A byte literal in hexadecimal form ($00-$FF)
// - A word literal in hexadecimal form ($0000-$FFFF)
// - A word literal in decimal form (0-65535)
// - A character in single quotes (e.g. 'A')

public class NumericParser
{
    private const string regexStringStandardValue = @"^(\$?)([0-9a-fA-F]{1,5})";
    private const string regexStringOffset = @"^(\$?)([0-9a-fA-F]{1,3})";
    
    private static Regex _regexStandardValue = new Regex(regexStringStandardValue, RegexOptions.Compiled);
    private static Regex _regexOffsetValue = new Regex(regexStringOffset, RegexOptions.Compiled);
    
    public static TokenParseResult<NumericValue> TryGetNumber(string operand, NumericValueKind kind = NumericValueKind.StandardValue)
    {
        var regex = kind switch
        {
            NumericValueKind.StandardValue => _regexStandardValue,
            NumericValueKind.DisplacementOffset => _regexOffsetValue,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
        var match = _regexStandardValue.Match(operand);
        if (match.Success)
        {
            var prefix = match.Groups[1].Value;
            var nbase = prefix switch
            {
                "$" => 16,
                _ => 10,
            };
            var number = match.Groups[2].Value;
            if (int.TryParse(number,  nbase == 16 ? NumberStyles.HexNumber : NumberStyles.None, CultureInfo.InvariantCulture, out var value))
            {
                var parsedValue =  new NumericValue(operand, value, value <= 255);
                return TokenParseResult<NumericValue>.Success(parsedValue);
            }
        }

        return TokenParseResult<NumericValue>.Error($"Value '{operand}' can't be parsed as a NumericValue");
    }

}