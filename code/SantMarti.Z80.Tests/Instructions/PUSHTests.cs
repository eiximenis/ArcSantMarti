using FluentAssertions;
using Microsoft.VisualBasic.CompilerServices;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Extensions;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class PUSHTests :InstructionTestsBase
{
    public PUSHTests()
    {
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
        Processor.Registers.SP = START_SP;
        var assembler = new Z80AssemblerBuilder();
        assembler.PUSH(register);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        Processor.Registers.SP.Should().Be(expectedSP);
    }
    
    [Theory()]
    [InlineData("HL")]
    [InlineData("AF")]
    [InlineData("BC")]
    [InlineData("DE")]
    public async Task PUSH_RR_Should_Take_11_Ticks(string register)
    {
        const int EXPECTED_TICKS = 11;
        Processor.Registers.SP = 20;
        var assembler = new Z80AssemblerBuilder();
        assembler.PUSH(register);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        TickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }
    [Theory()]
    [InlineData("HL", "H", "L")]
    [InlineData("AF", "A", "F")]
    [InlineData("BC", "B", "C")]
    [InlineData("DE", "D", "E")]
    public async Task PUSH_RR_Should_Write_RR_On_Stack(string register, string firstHalf, string secondHalf)
    {
        Processor.Registers.SP = 20;
        var random = new Random();
        Processor.Registers.Main.HL = 0x1234;
        Processor.Registers.Main.AF = 0x4321;
        Processor.Registers.Main.BC = 0x9876;
        Processor.Registers.Main.DE = 0xabcd;
        var assembler = new Z80AssemblerBuilder();
        assembler.PUSH(register);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        TickHandler.MemoryWrites.Should().HaveCount(2);
        TickHandler.MemoryWrites.First().Data.Should()
            .Be(Processor.Registers.GetByteRegisterByName(firstHalf));
        TickHandler.MemoryWrites.First().Address.Should().Be(19);
        TickHandler.MemoryWrites.Last().Data.Should()
            .Be(Processor.Registers.GetByteRegisterByName(secondHalf));
        TickHandler.MemoryWrites.Last().Address.Should().Be(18);
    }
    
}