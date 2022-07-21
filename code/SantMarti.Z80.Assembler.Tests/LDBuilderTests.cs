using FluentAssertions;
using SantMarti.Z80.Assembler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Tests
{
    public class LDBuilderTests
    {

        [Theory]
        [InlineData("A", "D", 0x7a)]
        [InlineData("D", "A", 0x57)]
        [InlineData("H", "E", 0x63)]
        [InlineData("E", "E", 0x5b)]
        [InlineData("B", "C", 0x41)]
        [InlineData("C", "L", 0x4d)]
        [InlineData("L", "B", 0x68)]
        public void LD_With_Two_ByteRegisters_Should_Generate_Valid_Opcode(string target, string source, byte expectedOpcode)
        {
            var builder = new Z80AssemblerBuilder();
            builder.LD(target, source);
            var asm = builder.Build();
            asm.Should().HaveCount(1);
            asm.First().Should().Be(expectedOpcode);
        }

        [Theory]
        [InlineData("A", "D", 0x7a)]
        [InlineData("D", "A", 0x57)]
        [InlineData("H", "E", 0x63)]
        [InlineData("E", "E", 0x5b)]
        [InlineData("B", "C", 0x41)]
        [InlineData("C", "L", 0x4d)]
        [InlineData("L", "B", 0x68)]
        public void LD_RR_With_Two_ByteRegisters_Should_Generate_Valid_Opcode(string target, string source, byte expectedOpcode)
        {
            var builder = new Z80AssemblerBuilder();
            builder.LD_RR(target, source);
            var asm = builder.Build();
            asm.Should().HaveCount(1);
            asm.First().Should().Be(expectedOpcode);
        }

        [Theory]
        [InlineData("LD A,D", 0x7a)]
        [InlineData("LD D,A", 0x57)]
        [InlineData("LD H,E", 0x63)]
        [InlineData("LD E,E", 0x5b)]
        [InlineData("LD B,C", 0x41)]
        [InlineData("LD C,L", 0x4d)]
        [InlineData("LD L,B", 0x68)]
        public void Assembly_Line_With_LD_With_Two_ByteRegisters_Should_Generate_Valid_Opcode(string line, byte expectedOpcode)
        {
            var builder = new Z80AssemblerBuilder();
            builder.Asm(line);
            var asm = builder.Build();
            asm.Should().HaveCount(1);
            asm.First().Should().Be(expectedOpcode);
        }
    }
}
