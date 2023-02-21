using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders
{
    static class LDBuilder 
    {

        public static AssemblerLineResult LD(string dest, string source)
        {
            var destRegister = RegisterParser.TryGetRegister(dest);
            var sourceRegister = RegisterParser.TryGetRegister(source);
            
            if (destRegister.HasError) return AssemblerLineResult.Error($"Invalid destination: {dest}");
            if (sourceRegister.HasError) return AssemblerLineResult.Error($"Invalid source: {source}");

            return LD(destRegister.ParsedToken!, sourceRegister.ParsedToken!);
        }
        
        public static AssemblerLineResult LD(RegisterReference dest, RegisterReference source)
        {
            if (dest.IsByteRegister)
            {
                if (source.IsByteRegister)
                {
                    return LD_RR(dest, source);
                }
            }

            return AssemblerLineResult.Error($"Invalid combination of registers ${dest.StrValue}, ${source.StrValue}");
        }

        internal static AssemblerLineResult LD(BaseToken dest, BaseToken source)
        {
            var destRegister = dest as RegisterReference;
            var sourceRegister = source as RegisterReference;
            
            if (destRegister is null || sourceRegister is null)
            {
                return AssemblerLineResult.Error(
                    $"Invalid LD operands {dest.StrValue} ({dest.Type}) or {source.StrValue} ({source.Type})");
            }

            return LD(destRegister, sourceRegister);
        }


        public static AssemblerLineResult LD_RR(string dest, string source)
        {
            var destRegister = RegisterParser.TryGetRegister(dest);
            var sourceRegister = RegisterParser.TryGetRegister(source);
            
            if (destRegister.HasError) return AssemblerLineResult.Error($"Invalid destination: {dest}");
            if (sourceRegister.HasError) return AssemblerLineResult.Error($"Invalid source: {source}");
            
            return LD_RR(destRegister.ParsedToken!, sourceRegister.ParsedToken!);
        }

        public static AssemblerLineResult LD_RR(RegisterReference dest, RegisterReference source)
        {
            if (!dest.IsByteRegister)
            {
                return AssemblerLineResult.Error($"Target {dest.StrValue} is not a byte register", dest);
            }
            if (!source.IsByteRegister)
            {
                return AssemblerLineResult.Error($"Source {source.StrValue} is not a byte register", source);
            }
            
            // LD r,r' opcode is 01DDDSSS (DDD = Destination, SSS = Source
            var destBits = RegistersEncoder.RegisterNameToBinaryValue(dest.StrValue);
            var sourceBits = RegistersEncoder.RegisterNameToBinaryValue(source.StrValue);
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
