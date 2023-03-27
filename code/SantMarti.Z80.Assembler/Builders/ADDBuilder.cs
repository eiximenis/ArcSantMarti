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
            var token1 = AnyParser.ParseToken(operand1);
            var token2 = AnyParser.ParseToken(operand2);

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
                        RegisterReference { IsByteRegister: true, IsGeneric: true } r => ADD_A_R(r),
                        NumericValue { IsByte: true } num => ADD_A_N(num),
                        MemoryReference { SourceRegisterName: "HL" } => ADD_A_HLRef(),
                        Displacement d => ADD_A_IX_IYDisp(d),
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
        
        public static AssemblerLineResult ADD_A_N(NumericValue value)
        {
            return AssemblerLineResult.Success(Z80Opcodes.ADD_A_N, value.AsByte());
        }

        private static AssemblerLineResult ADD_A_R(RegisterReference target)
        {
            var regValue = RegistersEncoder.ByteRegisterNameToBinaryValue(target.StrValue);
            return AssemblerLineResult.Success((byte)(Z80Opcodes.Bases.ADD_A_R | regValue));
        }
        
        // Builds:
        // 1. ADD A,(IX + d)
        // 2. ADD A,(IY + d)
        private static AssemblerLineResult ADD_A_IX_IYDisp(Displacement displacement)
        {
            var opcodes =  displacement.Register switch
            {
                "IX" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_AIXIY, (byte)displacement.Value },
                "IY" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_AIXIY, (byte)displacement.Value },
                _ => null
            };
                        
            return opcodes != null 
                ?  AssemblerLineResult.Success(opcodes) 
                : AssemblerLineResult.Error($"Invalid displacement {displacement.StrValue}",displacement);
        }
        
        // ADD A,(HL)
        public static AssemblerLineResult ADD_A_HLRef()
        {
            return AssemblerLineResult.Success(new [] {Z80Opcodes.ADD_A_HLRef}); 
        }
    }
}
