using System.Runtime.InteropServices.ComTypes;
using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class DAATests : InstructionTestsBase
{
    private const ushort TEST_START_ADDRESS = 20;
    
    public DAATests() : base(TEST_START_ADDRESS)
    {
    }
    

    // We rely on ADD for easy testing of DAA
    // Also: What is the supposed behavior of DAA when Acc value is not BCD-valid?
    [Theory]
    [InlineData(0x1, 0x3, 0x4, false)]
    [InlineData(0x2, 0x11, 0x13, false)]
    [InlineData(0x10, 0x22, 0x32, false)]
    [InlineData(0x9, 0x10, 0x19, false)]
    [InlineData(0x66, 0x66, 0x32, true)]        // 66 + 66 = 132 (0x132 is 32 with carry)
    [InlineData(0x39, 0x61, 0x00, true)]        // 39 + 61 = 100 (0x100 is 00 with carry)
    [InlineData(0x42, 0x72, 0x14, true)]        // 42 + 72 = 114 (0x114 is 14 with carry)
    [InlineData(0x99, 0x2, 0x01, true)]         // 99 + 2 = 101 (0x101 is 01 with carry)
    public async Task DAA_After_ADD_Should_Leave_BCD_Value_In_Acc(byte a, byte b,  byte result, bool carryExpected)
    {
        const int EXPECTED_TICKS = 4;
        Processor.Registers.Main.A = a;
        var assembler = new Z80AssemblerBuilder();
        assembler.ADD("A", b.ToString());
        assembler.DAA();
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        TickHandler.ResetTicks();
        await Processor.RunOnce();
        Processor.Registers.Main.A.Should().Be(result);
        Processor.Registers.Main.HasFlag(Z80Flags.Carry).Should().Be(carryExpected);
        TickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }
    
}