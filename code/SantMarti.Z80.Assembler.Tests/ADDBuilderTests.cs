using FluentAssertions;
using SantMarti.Z80.Assembler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Tests
{
    public class ADDBuilderTests
    {
        [Theory]
        [InlineData(0x7a)]
        [InlineData(0x57)]
        [InlineData(0x1)]
        [InlineData(0xff)]
        public void ADDA_With_Single_Byte_Should_Generate_First_The_ADD_Opcode_Then_The_Byte(byte byteToAdd)
        {
            var builder = new Z80AssemblerBuilder();
            builder.ADD_AN(byteToAdd);
            var asm = builder.Build();
            asm.Should().HaveCount(2);
            asm.First().Should().Be(Z80Opcodes.ADD_AN);
            asm.Last().Should().Be(byteToAdd);
        }

        [Theory]
        [InlineData("$A0", 0xa0)]
        [InlineData("$FF", 0xff)]
        [InlineData("$09", 0x9)]
        [InlineData("$9", 0x9)]
        public void ADD_With_Hex_Specifier_Should_Generate_First_The_ADD_Opcode_Then_The_Byte_Decoded(string hexByte, byte decodedByte)
        {
            var builder = new Z80AssemblerBuilder();
            builder.ADD("A", hexByte);
            var asm = builder.Build();
            asm.Should().HaveCount(2);
            asm.First().Should().Be(Z80Opcodes.ADD_AN);
            asm.Last().Should().Be(decodedByte);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("09", 9)]
        [InlineData("9", 9)]
        [InlineData("67", 67)]
        [InlineData("129", 129)]
        public void ADD_With_Literal_Number_Should_Generate_First_The_ADD_Opcode_Then_The_Byte_Value(string literal, byte byteValue)
        {
            var builder = new Z80AssemblerBuilder();
            builder.ADD("A", literal);
            var asm = builder.Build();
            asm.Should().HaveCount(2);
            asm.First().Should().Be(Z80Opcodes.ADD_AN);
            asm.Last().Should().Be(byteValue);
        }

        [Theory]
        [InlineData("IX", "0", 0xdd, 0)]
        [InlineData("IY", "$0", 0xfd, 0)]
        [InlineData("IX", "$AD", 0xdd, 0xad)]
        [InlineData("IY", "129", 0xfd, 129)]


        public void ADD_With_Displacement_Should_Generate_The_Correct_Prefix_The_Opcode_And_The_Byte_Value(string index, string encodedByte, byte expectedPrefix, byte byteValue)
        
        {
            var builder = new Z80AssemblerBuilder();
            builder.ADD("A", $"({index} + {encodedByte})");
            var asm = builder.Build();
            asm.Should().HaveCount(3);
            asm.First().Should().Be(expectedPrefix);
            asm.Should().BeEquivalentTo(new[] { expectedPrefix, Z80Opcodes.ADD_AIXIY, byteValue });
        }



    }
}
