using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using SantMarti.Z80.Assembler;

namespace SantMarti.Z80.Tests.Instructions;

public class DJNZTests :  InstructionTestsBase
{
    private const int TICKS_NO_JUMP = 8;
    private const int TICKS_JUMP = 13;
    
    [Fact]
    public async Task DJNZ_Should_Not_Jump_If_B_Is_Zero()
    {
        var assembler = new Z80AssemblerBuilder();
        
        assembler.DJNZ("$F0");
        Processor.Registers.Main.B = 0x1;
        SetupProcessorWithProgram(assembler);   
        await Processor.RunOnce();
        Processor.Registers.PC.Should().Be((ushort)(StartProgramAddress + 2));
        TickHandler.TotalTicks.Should().Be(TICKS_NO_JUMP);
    }
}