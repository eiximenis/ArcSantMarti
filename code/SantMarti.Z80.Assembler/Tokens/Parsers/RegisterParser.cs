using System.Reflection.Metadata.Ecma335;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public static class RegisterParser
{
    public static TokenParseResult<RegisterReference> TryGetRegister(string operand)
    {
        return operand switch
        {
            "A" or "B" or "C" or "D" or "E" or "H" or "L" => 
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, isByteRegister: true, isIndex: false)),
            "AF" or "BC" or "DE" or "HL" => 
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, isByteRegister: false, isIndex: false)),
            "IX" or "IY" => 
                TokenParseResult<RegisterReference>.Success(new RegisterReference(operand, isByteRegister: false, isIndex: true)),
            _ => TokenParseResult<RegisterReference>.Error($"Invalid register: {operand}")
        };
    }
    
    
}