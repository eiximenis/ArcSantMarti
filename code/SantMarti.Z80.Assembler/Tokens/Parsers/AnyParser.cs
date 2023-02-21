namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public class AnyParser
{
    public static BaseToken ParseToken(string token)
    {
        var register = RegisterParser.TryGetRegister(token);
        if (register.HasValue) return register.ParsedToken!;
        var memref = MemoryReferenceParser.TryGetMemoryReference(token);
        if (memref.HasValue) return memref.ParsedToken!;
        var number = NumericParser.TryGetNumber(token);
        if (number.HasValue) return number.ParsedToken!;
        var displacement = DisplacementParser.TryGetDisplacement(token);
        if (displacement.HasValue) return displacement.ParsedToken!;
        return new UnknownToken(token);
    }
}