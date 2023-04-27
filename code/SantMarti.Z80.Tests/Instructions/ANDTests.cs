using System.Reflection.Metadata;
using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Extensions;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class ANDTests : InstructionTestsBase
{
    
    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public async Task AND_R_Should_Do_Bitwise_And_Between_Acc_And_Register(string reg)
    {
        const int EXPECTED_TICKS = 4;
        var assembler = new Z80AssemblerBuilder();
        assembler.AND(reg);
        var rnd = new Random();
        var acc =rnd.GetRandomByte();
        var second = rnd.GetRandomByte();
        Processor.Registers.Main.A = acc;
        Processor.Registers.SetByteRegisterByName(reg, second);
        var expected = (byte)(Processor.Registers.Main.A & second);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        Processor.Registers.Main.A.Should().Be(expected);
        TickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }
}