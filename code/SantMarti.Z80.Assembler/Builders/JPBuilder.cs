using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

static class JPBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return JP(first);
    }
    
    public static AssemblerLineResult JP(string operand)
    {
        var token = AnyParser.ParseToken(operand);
        return JP(token);
    }

    public static AssemblerLineResult JP(BaseToken token)
    {
        return token switch
        {
            NumericValue { IsWord: true } value => JP_NN(value),
            _ => AssemblerLineResult.Error($"Invalid operand {token.StrValue}", token)
        };
    }
    
    private static AssemblerLineResult JP_NN(NumericValue value)
    {
        return AssemblerLineResult.Success(new byte[] { Z80Opcodes.JP_NN, value.LoByte(), value.HiByte() });
    }
}