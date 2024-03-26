using SantMarti.Z80.Assembler.Encoders;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Builders;
static class SUBBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var first = line.Operands[0];
        return SUB(first);
    }

    public static AssemblerLineResult SUB(string operand)
    {
        var first = AnyParser.ParseToken(operand, ParsersEnabled.ParametersToken);
        return SUB(first);
    }

    public static AssemblerLineResult SUB(BaseToken first)
    {
        return first switch
        {
            NumericValue { IsByte: true } nv => AssemblerLineResult.Success(Z80Opcodes.SUB_N, nv.AsByte()),
            RegisterReference { RegisterType: RegisterType.GenericByte } rref => AssemblerLineResult.Success((byte)(Z80Opcodes.Bases.SUB_R | RegistersEncoder.ByteRegisterNameToBinaryValue(rref.StrValue))),
            MemoryReference { SourceRegisterName: "HL" } => AssemblerLineResult.Success(Z80Opcodes.SUB_HLRef),
            Displacement { Register: "IX" } dispx => AssemblerLineResult.Success(Z80Opcodes.Prefixes.DD, Z80Opcodes.SUB_IX_IY, dispx.Value),
            Displacement { Register: "IY" } dispy => AssemblerLineResult.Success(Z80Opcodes.Prefixes.FD, Z80Opcodes.SUB_IX_IY, dispy.Value),
            _ => AssemblerLineResult.Error($"Invalid SUB operand ${first.StrValue}", first)
        };
    }
}
