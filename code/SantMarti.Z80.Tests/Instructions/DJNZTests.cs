using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Encoders;

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
        Processor.Registers.Main.B.Should().Be(0x0);
        Processor.NextFetchUsesWZ.Should().BeFalse();
        TickHandler.TotalTicks.Should().Be(TICKS_NO_JUMP);
    }
    
    [Theory]
    [InlineData(0xF0)]  // Negative offset
    [InlineData(0xFF)]  // Negative offset
    [InlineData(0xBA)]  // Positive offset
    [InlineData(0x10)]  // Positive offset
    public async Task DJNZ_Should_Jump_If_B_Is_Not_Zero(byte targetAddress)
    {
        var assembler = new Z80AssemblerBuilder();
        assembler.DJNZ(NumericEncoder.EncodeHexByte(targetAddress));
        Processor.Registers.Main.B = 0x2;
        SetupProcessorWithProgram(assembler);   
        await Processor.RunOnce();
        // As there is a Jump, WZ register is the one that has the new target address
        Processor.Registers.WZ.Should().Be((ushort)(StartProgramAddress + ((sbyte)targetAddress)));
        Processor.Registers.Main.B.Should().Be(0x1);
        Processor.Registers.PC.Should().Be((ushort)(Processor.Registers.WZ + 1));
        Processor.NextFetchUsesWZ.Should().BeTrue();
        TickHandler.TotalTicks.Should().Be(TICKS_JUMP);
    }
}