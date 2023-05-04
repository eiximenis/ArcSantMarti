using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class POPTests : InstructionTestsBase
{
    
    [Theory()]
    [InlineData("HL")]
    [InlineData("AF")]
    [InlineData("BC")]
    [InlineData("DE")]
    public async Task POP_RR_Should_Decrement_SP_On_Two(string register)
    {
        const ushort START_SP = 20;
        ushort expectedSP = START_SP  + 2;
        Processor.Registers.SP = START_SP;
        var assembler = new Z80AssemblerBuilder();
        assembler.POP(register);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        Processor.Registers.SP.Should().Be(expectedSP);
    }

    [Theory]
    [InlineData("HL")]
    [InlineData("AF")]
    [InlineData("BC")]
    [InlineData("DE")]
    public async Task POP_RR_Should_Read_Data_From_Memory_And_Left_In_Register(string register)
    {
        Processor.Registers.SP = 20;
        var rnd = new Random();
        Processor.Registers.SetWordRegisterByName(register, rnd.GetRandomUshort());
        var lobyte = rnd.GetRandomByte();
        var hibyte = rnd.GetRandomByte();
        TickHandler.OnMemoryRead(Processor.Registers.SP, lobyte);
        TickHandler.OnMemoryRead((ushort)(Processor.Registers.SP + 1), hibyte);
        var assembler = new Z80AssemblerBuilder();
        assembler.POP(register);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        TickHandler.TotalMemoryReads.Should().Be(2 + 1);        // 1 fetch + 2 read from stack
        var regValue = Processor.Registers.GetWordRegisterByName(register);
        regValue.Should().Be((ushort)((hibyte << 8) | lobyte));
    }

}