using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using SantMarti.Z80.Assembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Tests.Instructions;
public class SUBTests : InstructionTestsBase
{
    [Theory]
   
    [InlineData("250", "127", 123,false, false)]            // End value (123) is 0111_1011  -> No sign, no carry
    [InlineData("100", "101", 0b1111_1111, true, true)]     // End value (-1)  is 1_1111_1111  -> Carry enabled, sign enabled
    [InlineData("250", "10", 240, false, true)]             // End value (240) is 1111_0000 -> No carry, sign
    [InlineData("0", "180", 0b0100_1100, true, false)]      // End value (-180) is  1_0100_1010 (76) -> Carry, but no sign
    [InlineData("255", "0", 0b1111_1111, false, true)]      // End value (255) is  1111_1111 -> No carry, sign
    [InlineData("255", "1", 0b1111_1110, false, true)]      // End value (254) is  1111_1110  -> No carry, sign
    [InlineData("110", "120", 0b1111_0110, true, true)]     // End value (-10) is 1_1111_0110 (246) -> Carry, sign
    [InlineData("110", "250", 0b0111_0100, true, false)]    // End value (-240) is 1_0111_0100 (116) -> Carry, but no sign
    public async Task SUB_Should_Substract_Values_And_Set_Flags(string acc, string substract, byte resultExpected, bool carryExpected, bool signExpected)
    {
        var assembler = new Z80AssemblerBuilder();
        assembler.LD("A", acc);
        assembler.SUB(substract);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        await Processor.RunOnce();
        Processor.Registers.Main.A.Should().Be(resultExpected);
        Processor.Registers.Main.HasFlag(Z80Flags.Substract).Should().BeTrue();
        Processor.Registers.Main.HasFlag(Z80Flags.Carry).Should().Be(carryExpected);
        Processor.Registers.Main.HasFlag(Z80Flags.Sign).Should().Be(signExpected);
    }

    [Theory]
    [InlineData("B", "20", "10", 10)]
    [InlineData("C", "30", "10", 20)]
    [InlineData("D", "40", "50", 246)]
    public async Task SUB_R_Should_Substract_Value_From_Register(string register, string acc, string value, byte resultExpected)
    {
        var assembler = new Z80AssemblerBuilder();
        assembler.LD("A", acc);
        assembler.LD(register, value);
        assembler.SUB(register);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        await Processor.RunOnce();
        await Processor.RunOnce();
        Processor.Registers.Main.A.Should().Be(resultExpected);
    }
}
