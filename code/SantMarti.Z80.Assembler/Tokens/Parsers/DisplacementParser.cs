namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public class DisplacementParser
{
    public static TokenParseResult<Displacement> TryGetDisplacement(string data)
    {
        if (data[0] != '(' || data[data.Length - 1] != ')') { return TokenParseResult<Displacement>.Error($"Invalid displacement: {data}"); }

        data = data.Substring(1, data.Length - 2);
        var tokens = data.Split('+');
        if (tokens.Length != 2) { return TokenParseResult<Displacement>.Error($"Invalid displacement: {data}"); }
        var register = tokens[0].Trim();
        var displacementNumber = tokens[1].Trim();
        
        var number = NumericParser.TryGetNumber(displacementNumber);
        if (number.HasError || !number.ParsedToken!.IsByte)
        {
            return TokenParseResult<Displacement>.Error($"Displacement numeric value '{displacementNumber}' is not correct");
        }

        var displacement = new Displacement(data, register, number.ParsedToken!);
        return TokenParseResult<Displacement>.Success(displacement);
    }
}