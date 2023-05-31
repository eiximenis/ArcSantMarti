using System.Diagnostics;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

static class DECBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return DEC(first);
    }
    
    public static AssemblerLineResult DEC(string operand)
    {
        var token = AnyParser.ParseToken(operand);
        return DEC(token);
    }

    public static AssemblerLineResult DEC(BaseToken regToken)
    {
        return  regToken switch
        {
            RegisterReference { RegisterType: RegisterType.GenericByte } r => DEC_R(r),
            MemoryReference { SourceRegisterName: "HL" } => DEC_HLRef(),
            Displacement d => DEC_IX_IYDisp(d),
            _ => AssemblerLineResult.Error($"Invalid operand {regToken.StrValue}", regToken)
        };
    }

    private static AssemblerLineResult DEC_IX_IYDisp(Displacement displacement)
    {
        throw new NotImplementedException();
    }

    private static AssemblerLineResult DEC_HLRef()
    {
        throw new NotImplementedException();
    }

    private static AssemblerLineResult DEC_R(RegisterReference registerReference)
    {
        return registerReference switch
        {
            { StrValue: "A" } => AssemblerLineResult.Success(Z80Opcodes.DEC_A),
            { StrValue: "B" } => AssemblerLineResult.Success(Z80Opcodes.DEC_B),
            { StrValue: "C" } => AssemblerLineResult.Success(Z80Opcodes.DEC_C),
            { StrValue: "D" } => AssemblerLineResult.Success(Z80Opcodes.DEC_D),
            { StrValue: "E" } => AssemblerLineResult.Success(Z80Opcodes.DEC_E),
            { StrValue: "H" } => AssemblerLineResult.Success(Z80Opcodes.DEC_H),
            { StrValue: "L" } => AssemblerLineResult.Success(Z80Opcodes.DEC_L),
            _ => AssemblerLineResult.Error($"Invalid register {registerReference.StrValue}", registerReference)
        };
    }
}