using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders
{
    static class ADDBuilder
    {

        
        public static AssemblerLineResult BuildFromLine(TokenizedLine line)
        {
            var first = line.Operands[0];
            var second = line.Operands[1];
            return ADD(first, second);
        }
        
        public static AssemblerLineResult ADD(string operand1, string operand2)
        {
            var token1 = AnyParser.ParseToken(operand1, ParsersEnabled.ParametersToken);
            var token2 = AnyParser.ParseToken(operand2, ParsersEnabled.ParametersToken);

            return ADD(token1, token2);
        }

        public static AssemblerLineResult ADD(BaseToken first, BaseToken second)
        {
            switch (first)
            {
                // If first is a register, then we have ADD r, n
                case RegisterReference {StrValue: "A" }:
                    return second switch
                    {
                        RegisterReference { RegisterType: RegisterType.GenericByte } r => ADD_R(r),
                        NumericValue { IsByte: true } num => ADD_N(num),
                        MemoryReference { SourceRegisterName: "HL" } => ADD_HLRef(),
                        Displacement d => ADD_IX_IYDisp(d),
                        _ => AssemblerLineResult.Error($"Invalid second operand {second.StrValue}", second)
                    };
                case RegisterReference { StrValue: "HL" }:
                    return second switch
                    {
                        RegisterReference { StrValue: "BC" } => AssemblerLineResult.Success( Z80Opcodes.ADD_HL_BC ),
                        RegisterReference { StrValue: "DE" } => AssemblerLineResult.Success( Z80Opcodes.ADD_HL_DE ),
                        RegisterReference { StrValue: "HL" } => AssemblerLineResult.Success( Z80Opcodes.ADD_HL_HL ),
                        RegisterReference { StrValue: "SP" } =>  AssemblerLineResult.Success (Z80Opcodes.ADD_HL_SP ),
                        _ => AssemblerLineResult.Error($"Invalid second operand {second.StrValue}", second)
                    };
                default: 
                    return AssemblerLineResult.Error("Invalid first operand", first);
            }
        }
        
        public static AssemblerLineResult ADD_N(NumericValue value)
        {
            return AssemblerLineResult.Success(Z80Opcodes.ADD_A_N, value.AsByte());
        }

        private static AssemblerLineResult ADD_R(RegisterReference target)
        {
            var regValue = RegistersEncoder.ByteRegisterNameToBinaryValue(target.StrValue);
            return AssemblerLineResult.Success((byte)(Z80Opcodes.Bases.ADD_R | regValue));
        }
        
        // Builds:
        // 1. ADD A,(IX + d)
        // 2. ADD A,(IY + d)
        private static AssemblerLineResult ADD_IX_IYDisp(Displacement displacement)
        {
            var prefix = displacement.UseIX ? Z80Opcodes.Prefixes.DD : Z80Opcodes.Prefixes.FD;
            return AssemblerLineResult.Success(prefix, Z80Opcodes.ADD_AIXIY, displacement.Value);
        }
        
        // ADD A,(HL)
        public static AssemblerLineResult ADD_HLRef()
        {
            return AssemblerLineResult.Success(Z80Opcodes.ADD_A_HLRef); 
        }
    }
}
