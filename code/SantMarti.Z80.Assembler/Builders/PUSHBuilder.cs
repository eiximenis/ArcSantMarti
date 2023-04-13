using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

static class PUSHBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return PUSH(first);
    }
    
    public static AssemblerLineResult PUSH(string operand)
    {
        var token = AnyParser.ParseToken(operand);
        return PUSH(token);
    }
    
    public static AssemblerLineResult PUSH(BaseToken operand)
    {
        return operand switch
        {
            RegisterReference { StrValue: "AF" } => AssemblerLineResult.Success(Z80Opcodes.PUSH_AF),
            RegisterReference { StrValue: "BC" } => AssemblerLineResult.Success(Z80Opcodes.PUSH_BC),
            RegisterReference { StrValue: "DE" } => AssemblerLineResult.Success(Z80Opcodes.PUSH_DE),
            RegisterReference { StrValue: "HL" } => AssemblerLineResult.Success(Z80Opcodes.PUSH_HL),
            _ => AssemblerLineResult.Error($"Invalid operand {operand.StrValue}", operand)
        };
    }
}