using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Builders
{
    static class ADDBuilder
    {
        public static IEnumerable<byte> ADD_AN(byte value)
        {
            // ADD A,n has two bytes: opcode and byte value n
            yield return Z80Opcodes.ADD_AN;
            yield return value;
        }

        private static IEnumerable<byte>? ADD_AR(string target)
        {
            var regValue = RegistersEncoder.RegisterNameToBinaryValue(target);
            yield return (byte)(Z80Opcodes.Bases.ADD_AR | regValue);
        }

        public static byte[]? BuildFromLine(string keyword, string restLine)
        {
            var operands = restLine.Split(',');
            var target = operands[0].Trim();
            var source = operands[1].Trim();

            return ADD(source, target)?.ToArray();

        }
            
        internal static IEnumerable<byte>? ADD(string source, string target)
        {
            // source can be either "A", "HL", "IX" or "IY" and based on source target can have different values.
            switch (source)
            {
                case "A":
                    return ADD_A(target);
                case "HL":
                    return ADDHL(target);
                case "IX":
                    return ADDIX(target);
                case "IY":
                    return ADDIY(target);
                default:
                    return null;
            }


        }

        private static IEnumerable<byte>? ADD_A(string target)
        {
            if (HexEncoder.IsHexLiteral(target))
            {
                return ADD_AN(HexEncoder.HexLiteralToByte(target));
            }

            return target switch
            {
                "A" or "B" or "C" or "D" or "E" or "H" or "L" => ADD_AR(target),
                "(HL)" => ADD_AHL(),
                _ => null
            };
        }

        public static IEnumerable<byte> ADD_AHL()
        {
            yield return Z80Opcodes.ADD_AHL;   
        }

        public static IEnumerable<byte>? ADDHL(string target)
        {
            return target switch
            {
                "BC" => new byte[] { Z80Opcodes.ADD_HLBC },
                "DE" => new byte[] { Z80Opcodes.ADD_HLDE },
                "HL" => new byte[] { Z80Opcodes.ADD_HLHL },
                "SP" => new byte[] { Z80Opcodes.ADD_HLSP },
                _ => null
            };
        }

        public static IEnumerable<byte>? ADDIX(string target)
        {
            return target switch
            {
                "BC" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_IXBC },
                "DE" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_IXDE },
                "HL" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_IXIX },
                "SP" => new byte[] { Z80Opcodes.Prefixes.DD, Z80Opcodes.ADD_IXSP },
                _ => null
            };
        }

        private static IEnumerable<byte>? ADDIY(string target)
        {
            return target switch
            {
                "BC" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_IYBC },
                "DE" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_IYDE },
                "HL" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_IYIY },
                "SP" => new byte[] { Z80Opcodes.Prefixes.FD, Z80Opcodes.ADD_IYSP },
                _ => null
            };
        }



    }
}
