using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

static class JRBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return JR(first);
    }
    
    public static AssemblerLineResult JR(string operand)
    {
        var token = AnyParser.ParseToken(operand, ParsersEnabled.ParametersToken);
        return JR(token);
    }
    
    public static AssemblerLineResult JR(BaseToken token)
    {
        return token switch
        {
            NumericValue { IsByte: true } value => JR_D(value),
            _ => AssemblerLineResult.Error($"Invalid operand {token.StrValue}", token)
        };
    }

    private static AssemblerLineResult JR_D(NumericValue value)
    {
        return AssemblerLineResult.Success(Z80Opcodes.JR_D, value.AsByte());
    }
}