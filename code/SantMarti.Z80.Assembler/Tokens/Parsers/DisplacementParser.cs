using System.Text.RegularExpressions;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public class DisplacementParser
{
    private const string regexString = @"^\(\s*((IX|IY)\s*(\+|-)\s*([0-9]{1,3}))\s*\)";
    private static readonly Regex _regexCompiled = new(regexString, RegexOptions.Compiled);
    
    public static TokenParseResult<Displacement> TryGetDisplacement(string operand)
    {
        var match = _regexCompiled.Match(operand);
        if (match.Success)
        {
            var value = match.Groups[1].Value;
            var indexRegister = match.Groups[2].Value;              // Group 1 captures IX or IY
            var sign = match.Groups[3].Value;                       // Group 2 captures + or -
            var displacement = match.Groups[4].Value;               // Group 3 captures the displacement (3 digits max)

            if (!byte.TryParse(displacement, out var displacementByte))
            {
                return TokenParseResult<Displacement>.Error($"Invalid index displacement {operand}");
            }
            
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