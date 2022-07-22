using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Encoders
{
    public static class HexEncoder
    {

        private const string regexByteString = @"^\$[0-9a-fA-F]{1,2}$";               // $ followed by at most by two hexadecimal characters
        private const string regexWordString = @"^\$[0-9a-fA-F]{1,4}$";               // $ followed by at most by four hexadecimal characters

        private static Regex _regexByte;
        private static Regex _regexWord;


        static HexEncoder()
        {
            _regexByte = new Regex(regexByteString, RegexOptions.Compiled);
            _regexWord = new Regex(regexWordString, RegexOptions.Compiled);
        }


        public static bool IsHexByteLiteral(string literal) => IsHexLiteral(literal, 2);
        public static bool IsHexWordLiteral(string literal) => IsHexLiteral(literal, 4);

        // Check if string is in form:
        // $ABCD where ABCD are hexadecimal digits, and 
        private static bool IsHexLiteral(string literal, int maxdigits)
        {
            return maxdigits switch
            {
                2 => _regexByte.IsMatch(literal),
                4 => _regexWord.IsMatch(literal),
                _ => false
            };
        }
        public static byte HexLiteralToByte(string literal)
        {
            if (!IsHexLiteral(literal, 2)) {
                throw new InvalidOperationException($"Can't convert literal {literal} to byte");
            }

            var number = literal.Substring(1);
            var value = Convert.ToByte(number, 16);
            return value;
        }

        public static ushort HexLiteralToWord(string literal)
        {
            if (!IsHexLiteral(literal,  4))
            {
                throw new InvalidOperationException($"Can't convert literal {literal} to byte");
            }

            var number = literal.Substring(1);
            var value = Convert.ToUInt16(number, 16);
            return value;
        }
    }
}
