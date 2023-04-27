using System.Reflection.Metadata.Ecma335;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public static class RegisterParser
{
    public static TokenParseResult<RegisterReference> TryGetRegister(string operand)
    {
        return operand switch
        {
            "A" or "B" or "C" or "D" or "E" or "H" or "L" => 
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, RegisterType.GenericByte)),
            "I" or "R" =>
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, RegisterType.OtherByte)),
            "IXH" or "IXL" or "IYH" or "IYL" => 
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, RegisterType.IndexByte)),
            "AF" or "BC" or "DE" or "HL" =>
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, RegisterType.GenericWord)),
            "PC" or "SP" => 
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, RegisterType.OtherWord)),
            "IX" or "IY" => 
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, RegisterType.IndexWord)),
            _ => TokenParseResult<RegisterReference>.Error($"Invalid register: {operand}")
        };
    }
    
    
}