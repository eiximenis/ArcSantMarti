using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using SantMarti.Z80.Assembler;

namespace SantMarti.Z80.Tests.Instructions;

public class JPTests :  InstructionTestsBase
{
    [Fact]
    public async Task JP_NN_Should_Set_WZ_And_PC_Accordingly()
    {
        const int EXPECTED_TICKS = 10;
        var assembler = new Z80AssemblerBuilder();
        assembler.JP("$2002");
        SetupProcessorWithProgram(assembler);   
        await Processor.RunOnce();
        Processor.Registers.WZ.Should().Be(0x2002);
        Processor.Registers.PC.Should().Be(0x2002 + 1);
        Processor.NextFetchUsesWZ.Should().BeTrue();
        TickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }
    [Fact]
    public async Task JP_NN_Should_Make_A_Jump()
    { 
        var assembler = new Z80AssemblerBuilder();
        assembler.JP("$2002");
        TickHandler.OnMemoryRead(0x2002, Z80Opcodes.NOP);
        TickHandler.OnMemoryRead(0x2003, Z80Opcodes.NOP);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();                  // Run the JP. Next instruction should be NOP (0x2002)
        await Processor.RunOnce();
        TickHandler.MemoryReads.Should().Contain(0x2002);
        await Processor.RunOnce();
        TickHandler.MemoryReads.Should().Contain(0x2003);
        Processor.Registers.PC.Should().Be(0x2004);             // 0x2003 executed, ready to run next
    }

    [Fact]
    public async Task JP_PE_NN_Should_Make_A_Jump_Given_Parity_Flat_Is_Set()
    {
        var assembler = new Z80AssemblerBuilder();
        assembler.JP("$2002");
    }

}