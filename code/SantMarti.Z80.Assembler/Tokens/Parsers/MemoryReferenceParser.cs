using System.Diagnostics.CodeAnalysis;
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
    // A MemoryReference is:
    // A decimal (up to 5) digits inside parenthesis
    // A hexadecimal (up to 4) digits inside parenthesis and prefixed with $
    // A word register (HL, DE, BC, SP) inside parenthesis
    private const string regexString = @"^\(\s*(\$[0-9a-fA-F]{1,4}|[0-9]{1,5}|HL|DE|BC|SP)\s*\)";
    private static Regex _regex =  new Regex(regexString, RegexOptions.Compiled);
    
    public static TokenParseResult<MemoryReference> TryGetMemoryReference(string operand)
    {
        var match = _regex.Match(operand);
        if (match.Success)
        {
            var value = match.Groups[1].ValueSpan.ToString();

            return value switch
            {
                "HL" or "DE" or "BC" or "SP" => TokenParseResult<MemoryReference>.Success(
                    new MemoryReference(operand, RegisterParser.TryGetRegister(value).ParsedToken!)),
                _ when value.StartsWith('$') => ParseHexadecimalAddress(value),
                _ => ParseDecimalAddress(value)
            };
        }
        
        return TokenParseResult<MemoryReference>.Error($"Invalid memory reference {operand}");
    }

    private static TokenParseResult<MemoryReference> ParseHexadecimalAddress(string value)
    {
        return ushort.TryParse(value[1..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var parsedValue)
            ? TokenParseResult<MemoryReference>.Success(new MemoryReference(value, parsedValue))
            : TokenParseResult<MemoryReference>.Error($"Invalid hexadecimal address: {value}");
    }
    
    private static TokenParseResult<MemoryReference> ParseDecimalAddress(string value)
    {
        return ushort.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var parsedValue)
            ? TokenParseResult<MemoryReference>.Success(new MemoryReference(value, parsedValue))
            : TokenParseResult<MemoryReference>.Error($"Invalid decimal address: {value}");
    }

}