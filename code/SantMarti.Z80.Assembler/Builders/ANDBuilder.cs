using SantMarti.Z80.Assembler.Encoders;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

public class ANDBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return AND(first);
    }

    public static AssemblerLineResult AND(string operand)
    {
        var token = AnyParser.ParseToken(operand);
        return AND(token);
    }

    public static AssemblerLineResult AND(BaseToken regToken)
    {
        return regToken switch
        {
            RegisterReference { RegisterType: RegisterType.GenericByte } r => AND_R(r),
            RegisterReference {RegisterType: RegisterType.IndexByte} r => AND_I8(r),
            MemoryReference { SourceRegisterName: "HL" } => AssemblerLineResult.Success(Z80Opcodes.AND_HLRef),
            NumericValue {IsByte: true} n => AND_N(n),
            Displacement d => AND_IX_IYDisp(d),
            _ => AssemblerLineResult.Error($"Invalid operand {regToken.StrValue}", regToken)
        };
    }
    
    // AND IXH|IXL|IYH|IYL
    private static AssemblerLineResult AND_I8(RegisterReference register)
    {
        return register.StrValue switch
        {
            "IXH" => AssemblerLineResult.Success(Z80Opcodes.Prefixes.DD, Z80Opcodes.DD_AND_IXH),
            "IXL" => AssemblerLineResult.Success(Z80Opcodes.Prefixes.DD, Z80Opcodes.DD_AND_IXL),
            _ => AssemblerLineResult.Error($"Invalid operand (expected IXH|IXL|IYH|IYL){register.StrValue}", register)
        };
    }

    private static AssemblerLineResult AND_N(NumericValue number)
    {
        return AssemblerLineResult.Success(Z80Opcodes.AND_N, number.AsByte());
    }

    private static AssemblerLineResult AND_IX_IYDisp(Displacement displacement)
    {
        throw new NotImplementedException();
    }

    private static AssemblerLineResult AND_R(RegisterReference register)
    {
        return AssemblerLineResult.Success((byte)(Z80Opcodes.Bases.AND_R | RegistersEncoder.ByteRegisterNameToBinaryValue(register.StrValue)));
    }
    
    
}