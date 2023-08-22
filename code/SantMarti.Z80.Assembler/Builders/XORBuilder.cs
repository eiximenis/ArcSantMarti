using SantMarti.Z80.Assembler.Encoders;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

static class XORBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return XOR(first);
    }
    
    public static AssemblerLineResult XOR(string operand)
    {
        var token = AnyParser.ParseToken(operand, ParsersEnabled.ParametersToken);
        return XOR(token);
    }
    
    public static AssemblerLineResult XOR(BaseToken regToken)
    {
        return regToken switch
        {
            RegisterReference { RegisterType: RegisterType.GenericByte } r => XOR_R(r),
            _ => AssemblerLineResult.Error($"Invalid operand {regToken.StrValue}", regToken)
        };
    }

    private static AssemblerLineResult XOR_R(RegisterReference register)
    {
        return AssemblerLineResult.Success((byte)(Z80Opcodes.Bases.XOR_R | RegistersEncoder.ByteRegisterNameToBinaryValue(register.StrValue)));
    } 

}