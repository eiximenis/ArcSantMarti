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

    // Overflow flag is set when operands have both same sign and result has different sign.
    // In INC there is only *ONE* possibility to have the P/V flag set: 0x7f + 1 = 0x80
    // All others possibilites result in:
    // - Result has same sign of operands (when INC anything below 0x7f)
    // - Operands have DIFFERENT sign (when INC anything above 0x7f operand has sign and 0x1 hasn't)
    [Theory]
    [InlineData("A", 0x1, false)]
    [InlineData("A", 0x7e, false)]
    [InlineData("A", 0x7f, true)]
    [InlineData("A", 0x80, false)]
    [InlineData("A", 0xff, false)]
    public async Task INC_R_Should_Set_Overflow_Flag_If_Expected(string register, byte initial, bool overflowExpected)
    {
        _processor.Registers.SetByteRegisterByName(register, initial);
        var assembler = new Z80AssemblerBuilder();
        assembler.INC(register);
        SetupProcessorWithProgram(assembler);
        await _processor.RunOnce();
        _processor.Registers.Main.F.HasFlag(Z80Flags.ParityOrOverflow).Should().Be(overflowExpected);
    }
    
    

}