using System.Text.RegularExpressions;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public class DisplacementParser
{
    private const string regexString = @"^\(\s*((IX|IY)\s*(\+|-)\s*(.+))\s*\)";
    private static readonly Regex _regexCompiled = new(regexString, RegexOptions.Compiled);
    
    public static TokenParseResult<Displacement> TryGetDisplacement(string operand)
    {   
        var match = _regexCompiled.Match(operand);
        if (match.Success)
        {
            var value = match.Groups[1].Value;
            var indexRegister = match.Groups[2].Value;              // Group 1 captures IX or IY
            var sign = match.Groups[3].Value;                       // Group 2 captures + or -
            var displacement = match.Groups[4].Value;               // Group 3 captures the displacement
            var displacementParseResult = NumericParser.TryGetNumber(displacement, NumericValueKind.DisplacementOffset);

            if (displacementParseResult.HasError)
            {
                var errMsg = displacementParseResult.ErrorMessage;
                return TokenParseResult<Displacement>.Error($"Invalid displacement offset (error: {errMsg}");
            }

            var offset = displacementParseResult.ParsedToken!;
            if (offset.IsWord)
            {
                return TokenParseResult<Displacement>.Error($"Invalid displacement offset (must be a byte)");
            }

            var displacementByte = offset.AsByte();
            var displacedValue =sign == "-" ? (short)-displacementByte : displacementByte;
            if (displacedValue is < -128 or > 127)
            {
                return TokenParseResult<Displacement>.Error(
                    $"Invalid index displacement {operand} (must be between -128 and 127)");
            }

            return TokenParseResult<Displacement>.Success(new Displacement(operand, indexRegister, displacedValue));
        }
        
        return TokenParseResult<Displacement>.Error($"Invalid index displacement {operand}");
    }
}