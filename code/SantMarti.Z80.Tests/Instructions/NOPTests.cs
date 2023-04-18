using FluentAssertions;
using SantMarti.Z80.Assembler;

namespace SantMarti.Z80.Tests.Instructions;

public class NOPTests
{
    private readonly Z80Processor _processor;
    private readonly TestTickHandler _testTickHandler;
    private const ushort START_ADDRESS = 0x10;
    public NOPTests()
    {
        _processor = new Z80Processor();
        _testTickHandler = new TestTickHandler(_processor);
    }

    private void SetupProcessorWithProgram(Z80AssemblerBuilder asm)
    {
        var program = asm.Build();
        _testTickHandler.OnMemoryRead(START_ADDRESS, program);
        _processor.Registers.PC = START_ADDRESS;
    }
    
    [Fact]
    public async Task NOP_Should_Do_Nothing_But_Lasts_For_4_TStates()
    {
        const int EXPECTED_TICKS = 4;
        var assembler = new Z80AssemblerBuilder();
        assembler.NOP();
        SetupProcessorWithProgram(assembler);
        await _processor.RunOnce();
        _processor.Registers.PC.Should().Be(START_ADDRESS + 1);
        _testTickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }
}