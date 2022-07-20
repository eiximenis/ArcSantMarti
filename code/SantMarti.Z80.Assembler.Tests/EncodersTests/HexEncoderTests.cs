using FluentAssertions;
using SantMarti.Z80.Assembler.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Tests.EncodersTests
{
    public class HexEncoderTests
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
        public void HexEncoder_With_Valid_Value_Should_Return_Correct_Byte(string hexByte, byte expectedByte)
        {
            var result = HexEncoder.HexLiteralToByte(hexByte);
            result.Should().Be(expectedByte);
        }
    }
}
