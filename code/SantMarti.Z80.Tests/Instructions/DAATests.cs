using System.Runtime.InteropServices.ComTypes;
using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class DAATests
{
    private readonly Z80Processor _processor;
    private readonly TestTickHandler _testTickHandler;
    private const ushort TEST_START_ADDRESS = 20;
    
    public DAATests()
    {
        _processor = new Z80Processor();
        _testTickHandler = new TestTickHandler(_processor);
    }

    private void SetupProcessorWithProgram(Z80AssemblerBuilder asm)
    {
        var program = asm.Build();
        _testTickHandler.OnMemoryRead(10, program);
        _processor.Registers.PC = 10;
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
        _processor.Registers.Main.A = a;
        var assembler = new Z80AssemblerBuilder();
        assembler.ADD("A", b.ToString());
        assembler.DAA();
        _processor.SetupWithProgram(_testTickHandler, assembler, TEST_START_ADDRESS);
        await _processor.RunOnce();
        _testTickHandler.ResetTicks();
        await _processor.RunOnce();
        _processor.Registers.Main.A.Should().Be(result);
        _processor.Registers.Main.HasFlag(Z80Flags.Carry).Should().Be(carryExpected);
        _testTickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
    }
    
}