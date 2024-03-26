using SantMarti.Z80.Assembler.Encoders;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Builders;
static class BITBuilder
{

    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        var second = line.Operands[1];
        return BIT(first, second);
    }

    public static AssemblerLineResult BIT(string first, string second)
    {
        var firstToken = AnyParser.ParseToken(first, ParsersEnabled.Number);
        var secondToken = AnyParser.ParseToken(second, ParsersEnabled.ParametersToken);

        return BIT(firstToken, secondToken);
    }

    private static AssemblerLineResult BIT(BaseToken firstToken, BaseToken secondToken)
    {
        if (firstToken is NumericValue nv && nv.Value <= 7)
        {
            return secondToken switch
            {
                RegisterReference { RegisterType: RegisterType.GenericByte } rref => AssemblerLineResult.Success(Z80Opcodes.Prefixes.CB, (byte)(Z80Opcodes.Bases.BIT_N_R | (byte)(nv.Value << 3) | RegistersEncoder.ByteRegisterNameToBinaryValue(rref.StrValue))),
                MemoryReference { SourceRegisterName: "HL" } mref => AssemblerLineResult.Success(Z80Opcodes.Prefixes.CB, (byte)(Z80Opcodes.Bases.BIT_N_Others | (byte)(nv.Value << 3))),
                Displacement d => BIT_Displacement(nv, d),
                _ => AssemblerLineResult.Error($"Invalid BIT operand {secondToken.StrValue}", secondToken)
            };
        }
        else
        {
            return AssemblerLineResult.Error($"Invalid BIT operand {firstToken.StrValue}. Must be number between 0-7.", firstToken);
        }
    }

    private static AssemblerLineResult BIT_Displacement(NumericValue nv, Displacement disp)
    {
        var prefix = disp.UseIX ? Z80Opcodes.Prefixes.DD : Z80Opcodes.Prefixes.FD;
        return AssemblerLineResult.Success(prefix, Z80Opcodes.Prefixes.CB, (byte)(disp.Value), (byte)(Z80Opcodes.Bases.BIT_N_Others | (byte)(nv.Value << 3)));
    }
}
