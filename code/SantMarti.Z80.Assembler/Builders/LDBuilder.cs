using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Builders
{
    public static class LDBuilder 
    {

        public static byte[] Build(string dest, string source) => BuildBytes(dest, source);

        private static byte[] BuildBytes(string dest, string source)
        {            
            // LD opcode is 01DDDSSS (DDD = Destination, SSS = Source
            var destBits = RegistersEncoder.RegisterNameToBinaryValue(dest);
            var sourceBits = RegistersEncoder.RegisterNameToBinaryValue(source);

            var opcode = (byte)(Z80Opcodes.Bases.LD_XY | sourceBits  | (destBits << 3));

            return new[] { opcode };

        }

        public static byte[]? BuildFromLine(string keyword, string restLine)
        {
            var operands = restLine.Split(',');
            var target = operands[0].Trim();
            var source = operands[1].Trim();
            return BuildBytes(target, source);
        }
    }
}
