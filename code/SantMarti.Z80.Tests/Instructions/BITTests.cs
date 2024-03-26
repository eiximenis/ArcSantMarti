using FluentAssertions;
using SantMarti.Z80.Assembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Tests.Instructions;
public class BITTests : InstructionTestsBase
{
    [Fact]
    public async Task Bit_With_Displacement_Should_Take_20_TStates()
    {
        const int EXPECTED_TICKS = 20;
        var assembler = new Z80AssemblerBuilder();
        assembler.BIT("0", "(IX + 1)");
        SetupProcessorWithProgram(assembler);
        await Processor.RunOnce();
        TickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);

    }
}
