using Microsoft.Win32;
using SantMarti.Z80.Assembler.Encoders;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Builders;
static class CPBuilder
{
    public static AssemblerLineResult BuildFromLine(TokenizedLine line)
    {
        var operand = line.Operands[0];
        return CP(operand);
    }

    public static AssemblerLineResult CP(string operand)
    {
        var token =  AnyParser.ParseToken(operand, ParsersEnabled.ParametersToken);
        return CP(token);
    }

    public static AssemblerLineResult CP(BaseToken operand)
    {
        return operand switch
        {
            RegisterReference { RegisterType: RegisterType.GenericByte } register => AssemblerLineResult.Success((byte)(Z80Opcodes.Bases.CP_R | RegistersEncoder.ByteRegisterNameToBinaryValue(register.StrValue))),
            NumericValue {IsByte: true} nv => AssemblerLineResult.Success(Z80Opcodes.CP_N, nv.AsByte()),
            _ => AssemblerLineResult.Error($"Invalid CP operand {operand.StrValue}", operand)
        };
    }
}