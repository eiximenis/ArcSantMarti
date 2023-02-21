using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders
{
    static class LDBuilder 
    {
        /// <summary>
        /// Generates any LD instruction
        /// </summary>
        public static AssemblerLineResult LD(string dest, string source)
        {
            var destToken = AnyParser.ParseToken(dest);
            var srcToken = AnyParser.ParseToken(source);
            return LD(destToken, srcToken);
        }
        
        private static AssemblerLineResult LD(BaseToken dest, BaseToken source)
        {
            var destSrc = (dest, source);
            return destSrc switch
            {
                // Load memory reference over A or any 16 bit register 
                (RegisterReference reg, MemoryReference mr) when reg.AllowLoadMemoryReferences() => LD_MemRef(reg, mr),
                // Load any byte value over any byte register except I and R
                (RegisterReference {IsByteRegister: true} reg and not {StrValue: "I" or "R"}, NumericValue {IsByte: true} nv)  => LD_Byte(reg, nv),
                // Load any word value over BC, DE, HL or SP
                (RegisterReference {StrValue: "BC" or "DE" or "HL" or "SP" } reg, NumericValue {IsWord: true} nv) => LD_Word(reg,nv),
                // Load any byte register over any byte register
                (RegisterReference {StrValue: "A"} reg, RegisterReference { IsByteRegister: true} reg2) => LD_A(reg, reg2),
                // Load A, B,C, D, E, H, L over B,C, D, E, H, L 
                (RegisterReference {IsByteRegister: true} reg and not {StrValue: "A" or "I" or "R"}, RegisterReference { IsByteRegister: true} reg2 and not {StrValue: "I" or "R"}) => LD_RR8(reg, reg2),
                // Lood A over I or R
                (RegisterReference {StrValue: "I" or "R"} reg, RegisterReference { StrValue: "A"} reg2) => LD_IR(reg),
                _ => AssemblerLineResult.Error($"Unsupported combination of operands {dest}, {source}")
            };
        }
        
        // Generates LD I, A or LD R, A
        private static AssemblerLineResult LD_IR(RegisterReference target)
        {
            return target.StrValue switch
            {
                "I" => AssemblerLineResult.Success(new[] { Z80Opcodes.Prefixes.ED, Z80Opcodes.LD_IA }),
                "R" => AssemblerLineResult.Success(new[] { Z80Opcodes.Prefixes.ED, Z80Opcodes.LD_RA }),
                _ => AssemblerLineResult.Error($"Found invalid target {target}. Expected I or R", target)
            };
        }

        // Generates LD A,reg (reg is B, C, D, E, H, L, I, R)
        private static AssemblerLineResult LD_A(RegisterReference target, RegisterReference source)
        {
            return source.StrValue switch
            {
                "I" => AssemblerLineResult.Success(new[] { Z80Opcodes.Prefixes.ED, Z80Opcodes.LD_AI }),
                "R" => AssemblerLineResult.Success(new[] { Z80Opcodes.Prefixes.ED, Z80Opcodes.LD_AR }),
                _ => LD_RR8(target, source)
            };
        }

        // LD reg,nn (where reg is BC, DE, HL or SP
        private static AssemblerLineResult LD_Word(RegisterReference target, NumericValue source)
        {
            // LD r,nn opcode is 00DD0001 (DD = Destination)
            var bytes = RegistersEncoder.WordRegisterNameToBinaryValue(target.StrValue);
            var opcode = (byte)(Z80Opcodes.Bases.LD_RNN | (bytes << 4));
            return AssemblerLineResult.Success(new [] {opcode, source.LoByte(), source.HiByte()});
        }

        private static AssemblerLineResult LD_Byte(RegisterReference target, NumericValue source)
        {
            // LD r,n opcode is 00DDD110 (DDD = Destination, SSS = Source)
            var destBits = RegistersEncoder.ByteRegisterNameToBinaryValue(target.StrValue);
            var sourceBits = RegistersEncoder.ByteRegisterNameToBinaryValue(source.StrValue);
            var opcode = (byte)(Z80Opcodes.Bases.LD_RN | sourceBits | (destBits << 3));
            return AssemblerLineResult.Success(new [] {opcode, source.AsByte()});
        }

        private static AssemblerLineResult LD_MemRef(RegisterReference reg, MemoryReference mr)
        {
            return AssemblerLineResult.Error("Not yet implemented");
        }


        /// <summary>
        /// Generates a LD register, register instruction
        /// </summary>
        public static AssemblerLineResult LD_RR(string dest, string source)
        {
            var destRegister = RegisterParser.TryGetRegister(dest);
            var sourceRegister = RegisterParser.TryGetRegister(source);

            if (destRegister.HasError) return AssemblerLineResult.Error($"Invalid destination: {dest}");
            if (sourceRegister.HasError) return AssemblerLineResult.Error($"Invalid source: {source}");

            return LD_RR8(destRegister.ParsedToken!, sourceRegister.ParsedToken!);
        }
      
        private static AssemblerLineResult LD_RR8(RegisterReference dest, RegisterReference source)
        {
            // LD r,r' opcode is 01DDDSSS (DDD = Destination, SSS = Source)
            var destBits = RegistersEncoder.ByteRegisterNameToBinaryValue(dest.StrValue);
            var sourceBits = RegistersEncoder.ByteRegisterNameToBinaryValue(source.StrValue);
            var opcode = (byte)(Z80Opcodes.Bases.LD_RR | sourceBits | (destBits << 3));
            return AssemblerLineResult.Success(new [] {opcode});
        }
        
        public static AssemblerLineResult BuildFromLine(TokenizedLine line)
        {
            var target = line.Operands[0];
            var source = line.Operands[1];
            return LD(target, source);
        }
        
    }
}
