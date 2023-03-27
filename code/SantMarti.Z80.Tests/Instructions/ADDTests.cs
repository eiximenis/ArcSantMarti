using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Tests.Instructions
{
    public class ADDTests
    {
        private readonly Z80Processor _processor;
        private readonly TestTickHandler _testTickHandler;
        public ADDTests()
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

        [Theory]
        [InlineData(0x78, 0x69, 0xE1)]
        [InlineData(0xFF, 1, 0)]                // Carry and Overflow
        [InlineData(0x72, 0xFA, 0x6C)]          // Carry
        [InlineData(0x3e, 0x22, 0x60)]          // Half Carry
        [InlineData(0x39, 0x48, 0x81)]          // Half Carry
        public async Task ADDAN_Should_Add_Specified_Byte_Value_To_Accumulator(byte initialValue, byte valueToAdd, byte expectedResult)
        {
            const int EXPTECTED_TICKS = 7;
            _processor.Registers.Main.A = initialValue;                 // Initial accumulator value
            var assembler = new Z80AssemblerBuilder();
            assembler.ADD("A", valueToAdd.ToString());
            SetupProcessorWithProgram(assembler);
            await _processor.RunOnce();
            _testTickHandler.TotalTicks.Should().Be(EXPTECTED_TICKS);
            _processor.Registers.Main.A.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(0x78, 0x69, false)]
        [InlineData(0xFF, 1, true)]                // Carry and Overflow
        [InlineData(0x72, 0xFA, true)]          // Carry
        [InlineData(0x39, 0x48, false)]          // Half Carry

        public async Task ADDAN_Should_Set_Carry_Flag_When_Value_Exceeds_Maximum_Byte_Value(byte initialValue, byte valueToAdd, bool carryExpected)
        {
            const int EXPTECTED_TICKS = 7;
            _processor.Registers.Main.A = initialValue;                 // Initial accumulator value
            var assembler = new Z80AssemblerBuilder();
            assembler.ADD("A", valueToAdd.ToString());
            SetupProcessorWithProgram(assembler);
            await _processor.RunOnce();
            _testTickHandler.TotalTicks.Should().Be(EXPTECTED_TICKS);
            _processor.Registers.Main.F.HasFlag(Z80Flags.C).Should().Be(carryExpected);
        }

        [Theory]
        [InlineData(0x3e, 0x22, true)]           // 0x3e = 0011 1110
                                                 // 0x22 = 0010 0010
                                                 // (+)  = 0110 0000     = 0x60
                                                 // c    =   ** ***
        [InlineData(0x39, 0x48, true)]
        [InlineData(0xff, 1, true)]
        [InlineData(0b01010101, 0b10101010, false)]
        public async Task ADDAN_Should_Set_HalfCarryFlag_When_Intermediate_Carry_Is_Observed(byte initialValue, byte valueToAdd, bool halfCarryExpected)
        {
            const int EXPTECTED_TICKS = 7;
            _processor.Registers.Main.A = initialValue;                 // Initial accumulator value
            var assembler = new Z80AssemblerBuilder();
            assembler.ADD("A", valueToAdd.ToString());
            SetupProcessorWithProgram(assembler);
            await _processor.RunOnce();
            _testTickHandler.TotalTicks.Should().Be(EXPTECTED_TICKS);
            _processor.Registers.Main.F.HasFlag(Z80Flags.H).Should().Be(halfCarryExpected);
        }
        [Theory]
        [InlineData(0x78, 0x69)]
        [InlineData(0xFF, 1)]                // Carry and Overflow
        [InlineData(0x72, 0xFA)]          // Carry
        [InlineData(0x39, 0x48)]
        public async Task ADDAN_Should_Clear_SubstractionFlag_Always(byte initialValue, byte valueToAdd)
        {
            const int EXPTECTED_TICKS = 7;
            _processor.Registers.Main.SetFlag(Z80Flags.N);
            _processor.Registers.Main.A = initialValue;                 // Initial accumulator value
            var assembler = new Z80AssemblerBuilder();
            assembler.ADD("A", valueToAdd.ToString());
            SetupProcessorWithProgram(assembler);
            await _processor.RunOnce();
            _testTickHandler.TotalTicks.Should().Be(EXPTECTED_TICKS);
            _processor.Registers.Main.F.HasFlag(Z80Flags.N).Should().Be(false);
        }

        [Theory]
        [InlineData(0x0, 0x0, true)]
        [InlineData(0xFF, 1, true)]                
        [InlineData(0x72, 0xFA, false)]
        [InlineData(0xFF, 0xFF, false)]
        public async Task ADDAN_Should_Set_Zero_Flag_If_Accumulator_Is_Zero(byte initialValue, byte valueToAdd, bool zeroExpected)
        {
            const int EXPTECTED_TICKS = 7;
            _processor.Registers.Main.ClearFlag(Z80Flags.Z);
            _processor.Registers.Main.A = initialValue;                 // Initial accumulator value
            var assembler = new Z80AssemblerBuilder();
            assembler.ADD("A", valueToAdd.ToString());
            SetupProcessorWithProgram(assembler);
            await _processor.RunOnce();
            _testTickHandler.TotalTicks.Should().Be(EXPTECTED_TICKS);
            _processor.Registers.Main.F.HasFlag(Z80Flags.Z).Should().Be(zeroExpected);
        }
        

    }
}
