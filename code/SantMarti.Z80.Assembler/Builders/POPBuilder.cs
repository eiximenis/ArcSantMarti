using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

static class POPBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return POP(first);
    }
    
    public static AssemblerLineResult POP(string operand)
    {
        var token = AnyParser.ParseToken(operand, ParsersEnabled.ParametersToken);
        return POP(token);
    }
    
    public static AssemblerLineResult POP(BaseToken operand)
    {
        return operand switch
        {
            RegisterReference { StrValue: "AF" } => AssemblerLineResult.Success(Z80Opcodes.POP_AF),
            RegisterReference { StrValue: "BC" } => AssemblerLineResult.Success(Z80Opcodes.POP_BC),
            RegisterReference { StrValue: "DE" } => AssemblerLineResult.Success(Z80Opcodes.POP_DE),
            RegisterReference { StrValue: "HL" } => AssemblerLineResult.Success(Z80Opcodes.POP_HL),
            _ => AssemblerLineResult.Error($"Invalid operand {operand.StrValue}", operand)
        };
    }
    
}