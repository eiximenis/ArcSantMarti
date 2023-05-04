using FluentAssertions;
using SantMarti.Z80.Assembler;

namespace SantMarti.Z80.Tests.Instructions;

public class NOPTests : InstructionTestsBase
{
    public NOPTests()
    {
    }

    
    [Fact]
    public async Task NOP_Should_Do_Nothing_But_Lasts_For_4_TStates()
    {
        const int EXPECTED_TICKS = 4;
        var assembler = new Z80AssemblerBuilder();
        assembler.NOP();
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        Processor.Registers.PC.Should().Be((ushort)(StartProgramAddress + 1));
        TickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }
    
    [Fact]
    public async Task HALT_Should_Do_Nothing_But_Lasts_For_4_TStates()
    {
        const int EXPECTED_TICKS = 4;
        var assembler = new Z80AssemblerBuilder();
        assembler.HALT();
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        Processor.Registers.PC.Should().Be((ushort)(StartProgramAddress + 1));
        TickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }

    [Fact]
    public async Task HALT_Is_Reexecuted()
    {
        var assembler = new Z80AssemblerBuilder();
        assembler.HALT();
        // Don't put anything else
        SetupProcessorWithProgram(assembler);
        // Run some instructions more...
        await Processor.RunOnce();
        await Processor.RunOnce();
        await Processor.RunOnce();
        await Processor.RunOnce();
        await Processor.RunOnce();
        await Processor.RunOnce();
        // PC has been increased only once
        Processor.Registers.PC.Should().Be((ushort)(StartProgramAddress + 1));
    }

}