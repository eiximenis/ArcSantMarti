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
                // LD r,r
                (RegisterReference {IsByteRegister: true, IsGeneric: true} targetReg, RegisterReference { IsByteRegister: true, IsGeneric: true} srcReg) => LD_RR8(targetReg, srcReg),
                // LD r,n
                (RegisterReference {IsByteRegister: true, IsGeneric: true} targetReg, NumericValue {IsByte: true} srcValue) => LD_Byte(targetReg, srcValue),
                // LD r,(HL)
                (RegisterReference {IsByteRegister: true} targetReg, MemoryReference { SourceRegisterName: "HL"} srcMemRef) => LD_R_HLRef(targetReg, srcMemRef),
                // LD r,(IX|IY+d)
                (RegisterReference {IsByteRegister: true} targetReg, Displacement srcDis) => LD_R_Displacement(targetReg, srcDis),
                // LD (HL),r
                (MemoryReference { SourceRegisterName: "HL"} targetMemRef, RegisterReference {IsByteRegister: true} srcReg) => LD_HLRef_R(targetMemRef, srcReg),
                // LD (IX|IY+d),r 
                (Displacement targetDis, RegisterReference {IsByteRegister: true} srcReg) => LD_Displacement_R(targetDis, srcReg),
                // LD (HL),n
                (MemoryReference {SourceRegisterName: "HL"} targetMemRef, NumericValue {IsByte: true} srcValue) => LD_HLRef_Byte(targetMemRef, srcValue),
                // LD (IX|IY+d),n
                (Displacement targetDis, NumericValue {IsByte: true} srcValue) => LD_Displacement_Byte(targetDis, srcValue),
                // LD A,I|R
                (RegisterReference {StrValue: "A"} reg, RegisterReference { StrValue: "I" or "R"} reg2) => LD_A_IR(reg, reg2),
                // LD A, (nn) 
                (RegisterReference {StrValue: "A"} reg, MemoryReference {IsFixedAddress: true} mr) => LD_A(reg, mr),
                // LD dd,nn
                (RegisterReference {StrValue: "BC" or "DE" or "HL" or "SP" } reg, NumericValue {IsWord: true} nv) => LD_Word(reg,nv),
                // LD I|R,A
                (RegisterReference {StrValue: "I" or "R"} reg, RegisterReference { StrValue: "A"} reg2) => LD_IR_A(reg),
                
                // PENDING:
                // LD A,(BC|DE)
                // LD (BC|DE),A
                // LD (nn),A
                // LD IX|IY,nn
                // LD (HL),nn
                // LD dd,(nn)
                // LD IX|IY,(nn)
                // LD (nn),HL
                // LD (nn),dd
                // LD (nn), IX|IY
                // LD SP, HL
                // LD SP, IX|IY
                


                _ => AssemblerLineResult.Error($"Unsupported combination of operands {dest}, {source}")
            };
        }

        // Generates LD (HL),n
        private static AssemblerLineResult LD_HLRef_Byte(MemoryReference targetMemRef, NumericValue srcValue)
        {
            return AssemblerLineResult.Success(Z80Opcodes.LD_HLRef_Byte, srcValue.AsByte());
        }
        
        // Generates LD (IX+d),n or LD (IY+d),n
        private static AssemblerLineResult LD_Displacement_Byte(Displacement targetDis, NumericValue srcValue)
        {
            return AssemblerLineResult.Success(targetDis.Register == "IX" 
                    ? Z80Opcodes.Prefixes.DD 
                    : Z80Opcodes.Prefixes.FD, 
                Z80Opcodes.LD_Disp_Byte, srcValue.AsByte());
        }
        
        // Generates LD r,(IX+d) or LD r,(IY+d) where r is A, B, C, D, E, H, L
        private static AssemblerLineResult LD_R_Displacement(RegisterReference targetReg, Displacement srcDis)
        {
            var opcode = (byte)(Z80Opcodes.Bases.LD_R_Displacement | RegistersEncoder.ByteRegisterNameToBinaryValue(targetReg.StrValue) << 3);
            return AssemblerLineResult.Success(srcDis.Register == "IX" ?  Z80Opcodes.Prefixes.DD : Z80Opcodes.Prefixes.FD, opcode, (byte)srcDis.Value);
        }
        
        // Generates LD (IX + d),r  or LD (IY + d),r where r is A, B, C, D, E, H, L
        private static AssemblerLineResult LD_Displacement_R(Displacement targetDis, RegisterReference srcReg)
        {
            var opcode = (byte)(Z80Opcodes.Bases.LD_Displacement_R | RegistersEncoder.ByteRegisterNameToBinaryValue(srcReg.StrValue));
            return AssemblerLineResult.Success(targetDis.Register == "IX" ? Z80Opcodes.Prefixes.DD : Z80Opcodes.Prefixes.FD, opcode, (byte)targetDis.Value);
        }

        // Generate LD r,(HL) where r is A, B, C, D, E, H, L
        private static AssemblerLineResult LD_R_HLRef(RegisterReference target, MemoryReference source)
        {
            var opcode = (byte)(Z80Opcodes.Bases.LD_R_HLRef | RegistersEncoder.ByteRegisterNameToBinaryValue(target.StrValue) << 3);
            return AssemblerLineResult.Success(opcode);
        }
        
        // Generates LD (HL),r where r is A, B, C, D, E, H, L
        private static AssemblerLineResult LD_HLRef_R(MemoryReference targetMemRef, RegisterReference srcReg)
        {
            var opcode = (byte)(Z80Opcodes.Bases.LD_HLRef_R | RegistersEncoder.ByteRegisterNameToBinaryValue(srcReg.StrValue));
            return AssemblerLineResult.Success(opcode);
        }        
        
        // Generates LD I, A or LD R, A
        private static AssemblerLineResult LD_IR_A(RegisterReference target)
        {
            return target.StrValue switch
            {
                "I" => AssemblerLineResult.Success(new[] { Z80Opcodes.Prefixes.ED, Z80Opcodes.LD_IA }),
                "R" => AssemblerLineResult.Success(new[] { Z80Opcodes.Prefixes.ED, Z80Opcodes.LD_RA }),
                _ => AssemblerLineResult.Error($"Found invalid target {target}. Expected I or R", target)
            };
        }

        // Generates LD A, I|R
        private static AssemblerLineResult LD_A_IR(RegisterReference target, RegisterReference source)
        {

            var opcode = source.StrValue == "I" ? Z80Opcodes.LD_AI : Z80Opcodes.LD_AR;
            return AssemblerLineResult.Success(Z80Opcodes.Prefixes.ED, opcode);
        }
        
        // LD A,(nn)
        private static AssemblerLineResult LD_A(RegisterReference target, MemoryReference source)
        {
            return AssemblerLineResult.Success(new[] { Z80Opcodes.LD_A_NNRef, source.LoByte(), source.HiByte() });
        }

        // LD reg,nn (where reg is BC, DE, HL or SP
        private static AssemblerLineResult LD_Word(RegisterReference target, NumericValue source)
        {
            // LD r,nn opcode is 00DD0001 (DD = Destination)
            var bytes = RegistersEncoder.WordRegisterNameToBinaryValue(target.StrValue);
            var opcode = (byte)(Z80Opcodes.Bases.LD_R_NN | (bytes << 4));
            return AssemblerLineResult.Success(new [] {opcode, source.LoByte(), source.HiByte()});
        }

        private static AssemblerLineResult LD_Byte(RegisterReference target, NumericValue source)
        {
            // LD r,n opcode is 00DDD110 (DDD = Destination, SSS = Source)
            var destBits = RegistersEncoder.ByteRegisterNameToBinaryValue(target.StrValue);
            var sourceBits = RegistersEncoder.ByteRegisterNameToBinaryValue(source.StrValue);
            var opcode = (byte)(Z80Opcodes.Bases.LD_R_N | sourceBits | (destBits << 3));
            return AssemblerLineResult.Success(new [] {opcode, source.AsByte()});
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
            var opcode = (byte)(Z80Opcodes.Bases.LD_R_R | sourceBits | (destBits << 3));
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
