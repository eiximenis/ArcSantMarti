using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Builders
{
    static class LDBuilder 
    {

        internal static byte[]? LD(string dest, string source)
        {            

            if (RegistersEncoder.IsByteRegister(dest))
            {
                if (RegistersEncoder.IsByteRegister(source))
                {
                    return LD_RR(dest, source);
                }
            }

            return null;
        }

        public static byte[] LD_RR(string dest, string source)
        {
            // LD r,r' opcode is 01DDDSSS (DDD = Destination, SSS = Source
            var destBits = RegistersEncoder.RegisterNameToBinaryValue(dest);
            var sourceBits = RegistersEncoder.RegisterNameToBinaryValue(source);
            var opcode = (byte)(Z80Opcodes.Bases.LD_RR | sourceBits | (destBits << 3));
            return new[] { opcode };
        }

        public static byte[]? BuildFromLine(string keyword, string restLine)
        {
            var operands = restLine.Split(',');
            var target = operands[0].Trim();
            var source = operands[1].Trim();
            return LD(target, source);
        }
    }
}
