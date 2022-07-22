using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Encoders
{
    public static class NumericEncoder
    {
        public static bool IsNumericByteLiteral(string literal)
        {
            if (byte.TryParse(literal, out var data)) return true;
            return HexEncoder.IsHexByteLiteral(literal);
        }

        public static byte NumericLiteralToByte(string literal)
        {
            if (!IsNumericByteLiteral(literal))
            {
                throw new InvalidOperationException($"Can't convert literal {literal} to byte");
            }

            return byte.TryParse(literal, out var data) ? data : HexEncoder.HexLiteralToByte(literal);
        }


    }
}
