using FluentAssertions;
using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantMarti.Z80.Assembler.Tokens;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Tests.EncodersTests
{
    public class NumericParserTests
    {
        [Theory]
        [InlineData("$FF", 0xff)]
        [InlineData("$10", 0x10)]
        [InlineData("$AD", 0xad)]
        [InlineData("$A", 0xa)]
        [InlineData("$0A", 0xa)]
        [InlineData("$0", 0x0)]
        [InlineData("$00", 0x0)]
        [InlineData("$58", 0x58)]
        public void NumericParser_With_Valid_Byte_Value_Should_Return_Correct_Byte(string hexByte, byte expectedByte)
        {
            var result = NumericParser.TryGetNumber(hexByte).ParsedToken!.AsByte();
            result.Should().Be(expectedByte);
        }

        [Theory]
        [InlineData("$FF", 0xff)]
        [InlineData("$10", 0x10)]
        [InlineData("$AD", 0xad)]
        [InlineData("$A", 0xa)]
        [InlineData("$0A", 0xa)]
        [InlineData("$00A", 0xa)]
        [InlineData("$000A", 0xa)]
        [InlineData("$010", 0x10)]
        [InlineData("$EBCD", 0xebcd)]
        [InlineData("$158", 0x158)]
        [InlineData("$FFFF", 0xffff)]
        public void NumericParser_With_Valid_Word_Value_Should_Return_Correct_Word(string hexWord, ushort expectedWord)
        {
            var result = NumericParser.TryGetNumber(hexWord).ParsedToken!.Value;
            result.Should().Be(expectedWord);
        }

    }
}
