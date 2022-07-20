using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Encoders
{
    public static class HexEncoder
    {

        public static bool IsHexLiteral(string literal)
        {
            if (string.IsNullOrEmpty(literal) || literal.Length < 2)
            {
                return false;
            }
            var number = literal.Substring(1);

            if (literal[0] != '$' || string.IsNullOrEmpty(number) || number.Length > 2)
            {
                return false;
            }

            return true;

        }
        public static byte HexLiteralToByte(string literal)
        {
            if (!IsHexLiteral(literal)) {
                throw new InvalidOperationException($"Can't convert literal {literal} to byte");
            }

            var number = literal.Substring(1);
            var value = Convert.ToByte(number, 16);
            return value;
        }
    }
}
