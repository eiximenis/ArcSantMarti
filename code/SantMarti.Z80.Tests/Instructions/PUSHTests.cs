using FluentAssertions;
using Microsoft.VisualBasic.CompilerServices;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Extensions;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class PUSHTests
{
    private readonly Z80Processor _processor;
    private readonly TestTickHandler _testTickHandler;
    public PUSHTests()
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
    
    [Theory()]
    [InlineData("HL")]
    [InlineData("AF")]
    [InlineData("BC")]
    [InlineData("DE")]
    public async Task PUSH_RR_Should_Decrement_SP_On_Two(string register)
    {
        const ushort START_SP = 20;
        ushort expectedSP = START_SP - 2;
        _processor.Registers.SP = START_SP;
        var assembler = new Z80AssemblerBuilder();
        assembler.PUSH(register);
        SetupProcessorWithProgram(assembler);
        await _processor.RunOnce();
        _processor.Registers.SP.Should().Be(expectedSP);
    }
    
    [Theory()]
    [InlineData("HL")]
    [InlineData("AF")]
    [InlineData("BC")]
    [InlineData("DE")]
    public async Task PUSH_RR_Should_Take_11_Ticks(string register)
    {
        const int EXPECTED_TICKS = 11;
        _processor.Registers.SP = 20;
        var assembler = new Z80AssemblerBuilder();
        assembler.PUSH(register);
        SetupProcessorWithProgram(assembler);
        await _processor.RunOnce();
        _testTickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }
    [Theory()]
    [InlineData("HL", "H", "L")]
    [InlineData("AF", "A", "F")]
    [InlineData("BC", "B", "C")]
    [InlineData("DE", "D", "E")]
    public async Task PUSH_RR_Should_Write_RR_On_Stack(string register, string firstHalf, string secondHalf)
    {
        _processor.Registers.SP = 20;
        var random = new Random();
        _processor.Registers.Main.HL = 0x1234;
        _processor.Registers.Main.AF = 0x4321;
        _processor.Registers.Main.BC = 0x9876;
        _processor.Registers.Main.DE = 0xabcd;
        var assembler = new Z80AssemblerBuilder();
        assembler.PUSH(register);
        SetupProcessorWithProgram(assembler);
        await _processor.RunOnce();
        _testTickHandler.MemoryWrites.Should().HaveCount(2);
        _testTickHandler.MemoryWrites.First().Data.Should()
            .Be(_processor.Registers.GetByteRegisterByName(firstHalf));
        _testTickHandler.MemoryWrites.First().Address.Should().Be(19);
        _testTickHandler.MemoryWrites.Last().Data.Should()
            .Be(_processor.Registers.GetByteRegisterByName(secondHalf));
        _testTickHandler.MemoryWrites.Last().Address.Should().Be(18);
    }
    
    
}