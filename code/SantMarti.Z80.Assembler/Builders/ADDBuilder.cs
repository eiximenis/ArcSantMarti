using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Builders
{
    static class ADDBuilder
    {
        public static AssemblerLineResult ADD_AN(byte value)
        {
            // ADD A,n has two bytes: opcode and byte value n
            return AssemblerLineResult.Success(new [] {Z80Opcodes.ADD_AN, value});
        }
        public static AssemblerLineResult ADD_AN(NumericValue value)
        {
            if (!value.IsByte) return AssemblerLineResult.Error($"Value is not a byte: {value.StrValue}", value);
            return ADD_AN(value.AsByte());
        }

        private static AssemblerLineResult ADD_AR(RegisterReference target)
        {
            var regValue = RegistersEncoder.RegisterNameToBinaryValue(target.StrValue);
            return AssemblerLineResult.Success(new [] { (byte)(Z80Opcodes.Bases.ADD_AR | regValue)});
        }

        public static AssemblerLineResult BuildFromLine(TokenizedLine line)
        {
            var target = line.Operands[0];
            var source = line.Operands[1];
            return ADD(source, target);
        }

        internal static AssemblerLineResult ADD(BaseToken source, BaseToken target)
        {
            return source switch
            {
                RegisterReference r when r.StrValue == "A" => ADD_A(target),
                RegisterReference r when r.StrValue == "HL" => ADDHL(target),
                RegisterReference r when r.StrValue == "IX"  => ADDIX(target),
                RegisterReference r when r.StrValue == "IY" => ADDIY(target),
                _ =>  AssemblerLineResult.Error("Invalid source token", source)
            }; 
        }
            
        internal static AssemblerLineResult ADD(string source, string target)
        {
            var sourceToken = AnyParser.ParseToken(source);
            var destToken = AnyParser.ParseToken(target);
            return ADD(source, target);
        }
        
        
        private static AssemblerLineResult ADD_A(BaseToken target)
        {
            return target switch
            {
                NumericValue { IsByte: true } nv => ADD_AN(nv.AsByte()),
                RegisterReference r when r.IsByteRegister => ADD_AR(r),
                RegisterReference r when r.StrValue == "(HL)" => ADD_AHL(),
                Displacement d => ADD_Displacement(d),
                _ => AssemblerLineResult.Error($"Invalid target token {target.StrValue}", target)
            };
        }

        // Builds:
        // 1. ADD A,(IX + d)
        // 2. ADD A,(IY + d)
        private static AssemblerLineResult ADD_Displacement(Displacement displacement)
        {
            var opcodes =  displacement.Register switch
            {
                "IX" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_AIXIY, displacement.Number.AsByte() },
                "IY" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_AIXIY, displacement.Number.AsByte() },
                _ => null
            };
                        
            return opcodes != null 
                ?  AssemblerLineResult.Success(opcodes) 
                : AssemblerLineResult.Error($"Invalid displacement {displacement.StrValue}",displacement);
        }

        public static AssemblerLineResult ADD_AHL()
        {
            return AssemblerLineResult.Success(new [] {Z80Opcodes.ADD_AHL}); 
        }

        public static AssemblerLineResult ADDHL(BaseToken target)
        {
            return target switch
            {
                RegisterReference r when r.StrValue == "BC" => AssemblerLineResult.Success(new byte[] { Z80Opcodes.ADD_HLBC }),
                RegisterReference r when r.StrValue == "DE" => AssemblerLineResult.Success(new byte[] { Z80Opcodes.ADD_HLDE }),
                RegisterReference r when r.StrValue == "HL" => AssemblerLineResult.Success( new byte[] { Z80Opcodes.ADD_HLHL }),
                RegisterReference r when r.StrValue == "SP" =>  AssemblerLineResult.Success(new byte[] { Z80Opcodes.ADD_HLSP }),
                _ => AssemblerLineResult.Error("Invalid target token", target)
            };
        }


        private static AssemblerLineResult ADDIX(BaseToken target)
        {
            return  (target is RegisterReference targetRef) 
                ? ADDIX(targetRef)
                : AssemblerLineResult.Error($"Invalid target token {target.StrValue}", target);
        }
        
        public static AssemblerLineResult ADDIX(RegisterReference target)
        {
            if (!target.IsWordRegister)
            {
                return AssemblerLineResult.Error($"Invalid target register {target.StrValue}", target);
            }
            
            var opcodes =  target.StrValue switch
            {
                "BC" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_IXBC },
                "DE" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_IXDE },
                "HL" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_IXIX },
                "SP" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_IXSP },
                _ => null
            };

            return opcodes != null 
                ?  AssemblerLineResult.Success(opcodes) 
                : AssemblerLineResult.Error($"Invalid target register {target}");
        }

        
        private static AssemblerLineResult ADDIY(BaseToken target)
        {
            return  (target is RegisterReference targetRef) 
                ? ADDIY(targetRef)
                : AssemblerLineResult.Error($"Invalid target token {target.StrValue}", target);
        }
        private static AssemblerLineResult ADDIY(RegisterReference target)
        {

            if (!target.IsWordRegister)
            {
                return AssemblerLineResult.Error($"Invalid target register {target.StrValue}", target);
            }

            var opcodes =  target.StrValue switch
            {
                "BC" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_IYBC },
                "DE" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_IYDE },
                "HL" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_IYIY },
                "SP" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_IYSP },
                _ => null
            };
            
            return opcodes != null 
                ?  AssemblerLineResult.Success(opcodes) 
                : AssemblerLineResult.Error($"Invalid target register {target}");
        }
    }
}
