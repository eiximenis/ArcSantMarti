using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class INCTests
{
    private readonly Z80Processor _processor;
    private readonly TestTickHandler _testTickHandler;
    public INCTests()
    {
        _processor = new Z80Processor();
        _testTickHandler = new TestTickHandler(_processor);
    }

    private void SetupProcessorWithProgram(Z80AssemblerBuilder asm)
    {
        var program = asm.Build();
        _testTickHandler.OnMemoryRead(10, program);
        _processor.Registers.PC = 10;
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public async Task INC_R_Should_Increment_Register(string register)
    {
        const byte initial = 0x10;
        const byte expected = 0x11;
        _processor.Registers.SetByteRegisterByName(register, initial);
        var assembler = new Z80AssemblerBuilder();
        assembler.INC(register);
        SetupProcessorWithProgram(assembler);
        await _processor.RunOnce();
        _processor.Registers.GetByteRegisterByName(register).Should().Be(expected);
    }

}