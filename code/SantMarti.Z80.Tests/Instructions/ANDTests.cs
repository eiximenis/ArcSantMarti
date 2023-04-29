using System.Reflection.Metadata;
using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Extensions;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;
 
public class ANDTests : InstructionTestsBase
{
    
    [Theory]
    [InlineData("A", 4)]
    [InlineData("B", 4)]
    [InlineData("C", 4)]
    [InlineData("D", 4)]
    [InlineData("E", 4)]
    [InlineData("H", 4)]
    [InlineData("L", 4)]
    [InlineData("IXH", 8)]
    [InlineData("IXL", 8)]
    [InlineData("IYH", 8)]
    [InlineData("IYL", 8)]    
    public async Task AND_R_Or_I8_Should_Do_Bitwise_And_Between_Acc_And_Register(string reg, int expectedTicks)
    {
        var assembler = new Z80AssemblerBuilder();
        assembler.AND(reg);
        var rnd = new Random();
        var acc =rnd.GetRandomByte();
        var second = rnd.GetRandomByte();
        Processor.Registers.Main.A = acc;
        Processor.Registers.SetByteRegisterByName(reg, second);
        var expected = (byte)(Processor.Registers.Main.A & second);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        Processor.Registers.Main.A.Should().Be(expected);
        TickHandler.TotalTicks.Should().Be(expectedTicks);
    }
    
   
}