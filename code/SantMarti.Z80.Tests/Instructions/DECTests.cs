using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class DECTests : InstructionTestsBase
{
    
    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public async Task DEC_R_Should_Decrement_Register(string register)
    {
        const byte initial = 0x10;
        const byte expected = 0x0f;
        Processor.Registers.SetByteRegisterByName(register, initial);
        var assembler = new Z80AssemblerBuilder();
        assembler.DEC(register);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        Processor.Registers.GetByteRegisterByName(register).Should().Be(expected);
    }

}