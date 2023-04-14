using System.Diagnostics;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

public class INCBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return INC(first);
    }
    
    public static AssemblerLineResult INC(string operand)
    {
        var token = AnyParser.ParseToken(operand);
        return INC(token);
    }

    public static AssemblerLineResult INC(BaseToken regToken)
    {
        return  regToken switch
        {
            RegisterReference { IsByteRegister: true, IsGeneric: true } r => INC_R(r),
            MemoryReference { SourceRegisterName: "HL" } => INC_HLRef(),
            Displacement d => INC_IX_IYDisp(d),
            _ => AssemblerLineResult.Error($"Invalid operand {regToken.StrValue}", regToken)
        };
    }

    private static AssemblerLineResult INC_IX_IYDisp(Displacement displacement)
    {
        throw new NotImplementedException();
    }

    private static AssemblerLineResult INC_HLRef()
    {
        throw new NotImplementedException();
    }

    private static AssemblerLineResult INC_R(RegisterReference registerReference)
    {
        return registerReference switch
        {
            { StrValue: "A" } => AssemblerLineResult.Success(Z80Opcodes.INC_A),
            { StrValue: "B" } => AssemblerLineResult.Success(Z80Opcodes.INC_B),
            { StrValue: "C" } => AssemblerLineResult.Success(Z80Opcodes.INC_C),
            { StrValue: "D" } => AssemblerLineResult.Success(Z80Opcodes.INC_D),
            { StrValue: "E" } => AssemblerLineResult.Success(Z80Opcodes.INC_E),
            { StrValue: "H" } => AssemblerLineResult.Success(Z80Opcodes.INC_H),
            { StrValue: "L" } => AssemblerLineResult.Success(Z80Opcodes.INC_L),
            _ => AssemblerLineResult.Error($"Invalid register {registerReference.StrValue}", registerReference)
        };
    }
}