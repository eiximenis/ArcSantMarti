using FluentAssertions;
using SantMarti.Z80.Assembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Tests.Instructions;
public class CPTests : InstructionTestsBase
{
    [Theory]
    [InlineData("10","10", false)]            // sign(10) = 0, sign(10) = 0 => sign (10-10) = 0
    [InlineData("10", "8", false)]            // sign(10) = 0, sign(8) = 0 => sign (10-8) = 0
    [InlineData("10", "12", false)]           // sign(10) = 0, sign(12) = 0 => sign (10-8) = 0  
    [InlineData("10", "127", false)]          // sign(10) = 0, sign(127) = 0 => sign (10-127) = 1
    [InlineData("10", "129", true)]           // sign(10) = 0, sign(129) = 1 => sign (10-129) = 1 --> Overflow!!!!

    [InlineData("0", "1", false)]              // sign(0) = 0, sign(1) = 0 => sign (0-1) = 1ç
    [InlineData("0", "128", true)]            // sign(0) = 0, sign(128) = 1 => sign (0-128) = 1 --> Overflow!!!

    [InlineData("250", "10", false)]           // sign(250) = 1, sign(10) = 0 => sign(250-10) = 1
    [InlineData("250", "251", false)]          // sign(250) = 1, sign(251) = 1 => sign(250-251) = 1

    [InlineData("250", "127", true)]          // sign(250) = 1, sign(127) = 0 => sign(250-127) = 0 --> Overflow
    public async Task CP_Should_Set_Overflow_Flag_Accordingly(string acc, string substract, bool parityExpected)
    {
        var assembler = new Z80AssemblerBuilder();
        assembler.LD("A", acc);
        assembler.CP(substract);
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        await Processor.RunOnce();
        Processor.Registers.Main.HasFlag(Z80Flags.ParityOrOverflow).Should().Be(parityExpected);
    }

}
