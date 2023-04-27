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
                (RegisterReference {RegisterType: RegisterType.GenericByte} targetReg, RegisterReference { RegisterType: RegisterType.GenericByte} srcReg) => LD_R_R(targetReg, srcReg),
                // LD r,n
                (RegisterReference {RegisterType: RegisterType.GenericByte} targetReg, NumericValue {IsByte: true} srcValue) => LD_R_N(targetReg, srcValue),
                // LD r,(HL)
                (RegisterReference {RegisterType: RegisterType.GenericByte} targetReg, MemoryReference { SourceRegisterName: "HL"} srcMemRef) => LD_R_HLRef(targetReg, srcMemRef),
                // LD r,(IX|IY+d)
                (RegisterReference {RegisterType: RegisterType.GenericByte} targetReg, Displacement srcDis) => LD_R_Displacement(targetReg, srcDis),
                // LD (HL),r
                (MemoryReference { SourceRegisterName: "HL"} targetMemRef, RegisterReference {RegisterType: RegisterType.GenericByte} srcReg) => LD_HLRef_R(targetMemRef, srcReg),
                // LD (IX|IY+d),r 
                (Displacement targetDis, RegisterReference {RegisterType: RegisterType.GenericByte} srcReg) => LD_Displacement_R(targetDis, srcReg),
                // LD (HL),n
                (MemoryReference {SourceRegisterName: "HL"} targetMemRef, NumericValue {IsByte: true} srcValue) => LD_HLRef_Byte(targetMemRef, srcValue),
                // LD (IX|IY+d),n
                (Displacement targetDis, NumericValue {IsByte: true} srcValue) => LD_Displacement_Byte(targetDis, srcValue),
                // LD A,I|R
                (RegisterReference {IsAccumulator: true} reg, RegisterReference { RegisterType: RegisterType.OtherByte} reg2) => LD_A_IR(reg, reg2),
                // LD A,(nn) 
                (RegisterReference {IsAccumulator: true} reg, MemoryReference {IsFixedAddress: true} mr) => LD_A_NN(reg, mr),
                // LD (nn),A
                (MemoryReference {IsFixedAddress: true} mr, RegisterReference {IsAccumulator: true} reg) => LD_NN_A(mr, reg),
                // LD dd,nn
                (RegisterReference {StrValue: "BC" or "DE" or "HL" or "SP" } reg, NumericValue {IsWord: true} nv) => LD_DD_NN(reg,nv),
                // LD I|R,A
                (RegisterReference {RegisterType: RegisterType.OtherByte} reg, RegisterReference { IsAccumulator: true} reg2) => LD_IR_A(reg),
                // LD A,(BC|DE)
                (RegisterReference {IsAccumulator: true} reg, MemoryReference {SourceRegisterName: "BC" or "DE"} mr) => LD_A_BC_DERef(reg, mr),
                // LD (BC|DE),A
                (MemoryReference {SourceRegisterName: "BC" or "DE"} mr, RegisterReference {IsAccumulator: true} reg) => LD_BC_DERef_A(mr, reg),
                // LD IX|IY,nn
                (RegisterReference {RegisterType: RegisterType.IndexWord} reg, NumericValue {IsWord: true} nv) => LD_IX_IY_NN(reg, nv),
                // LD HL,(nn)
                (RegisterReference {StrValue: "HL"} reg, MemoryReference {IsFixedAddress: true} mr) => LD_HL_NNRef(reg, mr),
                // LD dd,(nn)
                (RegisterReference {StrValue: "BC" or "DE" or "SP" } reg, MemoryReference {IsFixedAddress: true} mr) => LD_DD_NNRef(reg, mr),
                // LD IX|IY,(nn)
                (RegisterReference {RegisterType: RegisterType.IndexWord} reg, MemoryReference {IsFixedAddress: true} mr) => LD_IX_IY_NNRef(reg, mr),
                // LD (nn),HL
                (MemoryReference {IsFixedAddress: true} mr, RegisterReference {StrValue: "HL"} reg) => LD_NNRef_HL(mr, reg),
                // LD (nn),dd
                (MemoryReference {IsFixedAddress: true} mr, RegisterReference {StrValue: "BC" or "DE" or "SP" } reg) => LD_NNRef_DD(mr, reg),
                // LD (nn), IX|IY
                (MemoryReference {IsFixedAddress: true} mr, RegisterReference {RegisterType: RegisterType.IndexWord} reg) => LD_NNRef_IX_IY(mr, reg),
                // LD SP, HL
                (RegisterReference {StrValue: "SP"} targetReg, RegisterReference {StrValue: "HL"} srcReg) => LD_SP_HL(targetReg, srcReg),
                // LD SP, IX|IY
                (RegisterReference {StrValue: "SP"} targetReg, RegisterReference {RegisterType: RegisterType.IndexWord} srcReg) => LD_SP_IX_IY(targetReg, srcReg),
                _ => AssemblerLineResult.Error($"Unsupported combination of operands {dest}, {source}")
            };
        }

        private static AssemblerLineResult LD_SP_IX_IY(RegisterReference targetReg, RegisterReference srcReg)
        {
            var prefix = srcReg.StrValue == "IX" ? Z80Opcodes.Prefixes.DD : Z80Opcodes.Prefixes.FD;
            return AssemblerLineResult.Success(prefix, Z80Opcodes.LD_SP_IX_IY);
            
        }

        private static AssemblerLineResult LD_SP_HL(RegisterReference targetReg, RegisterReference srcReg)
        {
            return AssemblerLineResult.Success(Z80Opcodes.LD_SP_HL);
        }

        private static AssemblerLineResult LD_NNRef_IX_IY(MemoryReference mr, RegisterReference reg)
        {
            var prefix = reg.StrValue == "IX" ? Z80Opcodes.Prefixes.DD : Z80Opcodes.Prefixes.FD;
            
            return AssemblerLineResult.Success(prefix, Z80Opcodes.LD_NNRef_IX_IY, mr.LoByte(), mr.HiByte());
        }

        // LD (nn), BC|DE|SP
        private static AssemblerLineResult LD_NNRef_DD(MemoryReference mr, RegisterReference reg)
        {
            var opcode = (byte)(Z80Opcodes.Bases.LD_NNRef_DD | RegistersEncoder.WordRegisterNameToBinaryValue(reg.StrValue) << 4);
            return AssemblerLineResult.Success(Z80Opcodes.Prefixes.ED, opcode, mr.LoByte(), mr.HiByte());
        }

        // LD (nn), HL
        private static AssemblerLineResult LD_NNRef_HL(MemoryReference mr, RegisterReference reg)
        {
            return AssemblerLineResult.Success(Z80Opcodes.LD_NNRef_HL, mr.LoByte(), mr.HiByte());
        }

        // LD IX|IY, (nn)
        private static AssemblerLineResult LD_IX_IY_NNRef(RegisterReference reg, MemoryReference mr)
        {
            var prefix = reg.StrValue == "IX" ? Z80Opcodes.Prefixes.DD : Z80Opcodes.Prefixes.FD;
            
            return AssemblerLineResult.Success(prefix, Z80Opcodes.LD_IX_IY_NNRef, mr.LoByte(), mr.HiByte());
        }

        // LD BC|DE|SP, (nn)
        private static AssemblerLineResult LD_DD_NNRef(RegisterReference reg, MemoryReference mr)
        {
            var opcode = (byte)(Z80Opcodes.Bases.LD_DD_NNRef |  RegistersEncoder.WordRegisterNameToBinaryValue(reg.StrValue) << 4);

            return AssemblerLineResult.Success(Z80Opcodes.Prefixes.ED, opcode, mr.LoByte(), mr.HiByte());
        }

        // LD HL,(nn)
        private static AssemblerLineResult LD_HL_NNRef(RegisterReference reg, MemoryReference mr)
        {
            return AssemblerLineResult.Success(Z80Opcodes.LD_HL_NNRef, mr.LoByte(), mr.HiByte());
        }

        // LD IX|IY,nn
        private static AssemblerLineResult LD_IX_IY_NN(RegisterReference target, NumericValue source)
        {

            var prefix = target.StrValue == "IX" ? Z80Opcodes.Prefixes.DD : Z80Opcodes.Prefixes.FD;
            
            return AssemblerLineResult.Success(prefix, Z80Opcodes.LD_IX_IY_NN, source.LoByte(), source.HiByte());
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
        private static AssemblerLineResult LD_A_NN(RegisterReference target, MemoryReference source)
        {
            return AssemblerLineResult.Success(new[] { Z80Opcodes.LD_A_NNRef, source.LoByte(), source.HiByte() });
        }
        
        // LD (nn), A
        private static AssemblerLineResult LD_NN_A(MemoryReference target, RegisterReference source)
        {
            return AssemblerLineResult.Success(new[] { Z80Opcodes.LD_NNRef_A, target.LoByte(), target.HiByte() });
        }
        
        // LD A,(BC|DE)
        private static AssemblerLineResult LD_A_BC_DERef(RegisterReference target, MemoryReference source)
        {
            var opcode = source.SourceRegisterName == "BC" ? Z80Opcodes.LD_A_BCRef : Z80Opcodes.LD_A_DERef;
            return AssemblerLineResult.Success(opcode);
        }
        
        // LD (BC|DE),A
        private static AssemblerLineResult LD_BC_DERef_A(MemoryReference target, RegisterReference source)
        {
            var opcode = target.SourceRegisterName == "BC" ? Z80Opcodes.LD_BCRef_A : Z80Opcodes.LD_DERef_A;
            return AssemblerLineResult.Success(opcode);
        }

        // LD reg,nn (where reg is BC, DE, HL or SP
        private static AssemblerLineResult LD_DD_NN(RegisterReference target, NumericValue source)
        {
            // LD r,nn opcode is 00DD0001 (DD = Destination)
            var bytes = RegistersEncoder.WordRegisterNameToBinaryValue(target.StrValue);
            var opcode = (byte)(Z80Opcodes.Bases.LD_R_NN | (bytes << 4));
            return AssemblerLineResult.Success(new [] {opcode, source.LoByte(), source.HiByte()});
        }

        private static AssemblerLineResult LD_R_N(RegisterReference target, NumericValue source)
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

            return LD_R_R(destRegister.ParsedToken!, sourceRegister.ParsedToken!);
        }
      
        private static AssemblerLineResult LD_R_R(RegisterReference dest, RegisterReference source)
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
