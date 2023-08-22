using SantMarti.Z80.Assembler.Encoders;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders;

static class ORBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return OR(first);
    }
    
    public static AssemblerLineResult OR(string operand)
    {
        var token = AnyParser.ParseToken(operand, ParsersEnabled.ParametersToken);
        return OR(token);
    }
    
    public static AssemblerLineResult OR(BaseToken regToken)
    {
        return regToken switch
        {
            RegisterReference { RegisterType: RegisterType.GenericByte } r => OR_R(r),
            _ => AssemblerLineResult.Error($"Invalid operand {regToken.StrValue}", regToken)
        };
    }

    private static AssemblerLineResult OR_R(RegisterReference register)
    {
        return AssemblerLineResult.Success((byte)(Z80Opcodes.Bases.OR_R | RegistersEncoder.ByteRegisterNameToBinaryValue(register.StrValue)));
    } 

}