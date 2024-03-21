using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;
using System.Security.Cryptography.X509Certificates;

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
        var token = AnyParser.ParseToken(operand, ParsersEnabled.ParametersToken);
        return JP(token);
    }

    public static AssemblerLineResult JP(string first, string second)
    {
        var firstToken = AnyParser.ParseToken(first, ParsersEnabled.ParametersToken);
        var secondToken = AnyParser.ParseToken(first, ParsersEnabled.ParametersToken);
        return JP(firstToken, secondToken);
    }

    public static AssemblerLineResult JP(BaseToken firstToken, BaseToken secondToken)
    {
        return (firstToken, secondToken) switch
        {
            (FlagReference { Flag: Z80ReferencedFlag.ParityOrOverflow, IsSet: true }, NumericValue { IsWord: true } nv) => JP_Opcode_NN(Z80Opcodes.JP_PE_NN, nv),
            _ => AssemblerLineResult.Error($"Invalid operand {firstToken.StrValue}", firstToken)
        };
    }
 
    public static AssemblerLineResult JP(BaseToken token)
    {
        return token switch
        {
            NumericValue { IsWord: true } value => JP_Opcode_NN(Z80Opcodes.JP_NN, value),
            _ => AssemblerLineResult.Error($"Invalid operand {token.StrValue}", token)
        };
    }

    private static AssemblerLineResult JP_Opcode_NN(byte baseOpcode, NumericValue value) => AssemblerLineResult.Success(baseOpcode, value.LoByte(), value.HiByte());

}