using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

public class DJNZBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return DJNZ(first);
    }
    
    public static AssemblerLineResult JP(string operand)
    {
        var token = AnyParser.ParseToken(operand);
        return DJNZ(token);
    }
    
    public static AssemblerLineResult DJNZ(BaseToken token)
    {
        return token switch
        {
            NumericValue { IsByte: true } value => DJNZ_N(value),
            _ => AssemblerLineResult.Error($"Invalid operand {token.StrValue}", token)
        };
    }
    
    private static AssemblerLineResult DJNZ_N(NumericValue value)
    {
        return AssemblerLineResult.Success(Z80Opcodes.DJNZ_N, value.AsByte());
    }
}