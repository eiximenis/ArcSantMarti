using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Encoders;

namespace SantMarti.Z80.Tests.Instructions;

public class JRTests :  InstructionTestsBase
{
    private const int TICKS_JUMP = 12;
    
    [Theory]
    [InlineData(0xF0)]  // Negative offset
    [InlineData(0xFF)]  // Negative offset
    [InlineData(0xBA)]  // Positive offset
    [InlineData(0x10)]  // Positive offset
    public async Task JR_Should_Jump_To_Relative_Offset(byte offset)
    {
        var assembler = new Z80AssemblerBuilder();
        assembler.JR(NumericEncoder.EncodeHexByte(offset));
        SetupProcessorWithProgram(assembler);   
        await Processor.RunOnce();
        // As there is a Jump, WZ register is the one that has the new target address
        Processor.Registers.WZ.Should().Be((ushort)(StartProgramAddress + ((sbyte)offset)));
        Processor.Registers.PC.Should().Be((ushort)(Processor.Registers.WZ + 1));
        Processor.NextFetchUsesWZ.Should().BeTrue();
        TickHandler.TotalTicks.Should().Be(TICKS_JUMP);
    }
    
}