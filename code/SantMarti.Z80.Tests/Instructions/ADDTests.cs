using FluentAssertions;
using SantMarti.Z80.Assembler;
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
        public ADDTests()
        {
            _processor = new Z80Processor();
        }

        [Theory]
        [InlineData(0x78, 0x69, 0xE1)]
        [InlineData(0xFF, 1, 0)]                // Carry and Overflow
        [InlineData(0x72, 0xFA, 0x6C)]          // Carry
        [InlineData(0x3e, 0x22, 0x60)]          // Half Carry
        [InlineData(0x39, 0x48, 0x81)]          // Half Carry
        public async Task ADDAN_Should_Add_Specified_Byte_Value_To_Accumulator(byte initialValue, byte valueToAdd, byte expectedResult)
        {
            _processor.Registers.Main.A = initialValue;                 // Initial accumulator value
            var assembler = new Z80AssemblerBuilder();
            assembler.ADDA(valueToAdd);
            var bus = new TestBusBuilder().Add(assembler.Build()).BuildBus();
            _processor.ConnectToDataBus(bus);
            await _processor.RunOnce();
            _processor.Registers.Main.A.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(0x78, 0x69, false)]
        [InlineData(0xFF, 1, true)]                // Carry and Overflow
        [InlineData(0x72, 0xFA, true)]          // Carry
        [InlineData(0x39, 0x48, false)]          // Half Carry

        public async Task ADDAN_Should_Set_Carry_Flag_When_Value_Exceeds_Maximum_Byte_Value(byte initialValue, byte valueToAdd, bool carryExpected)
        {
            _processor.Registers.Main.A = initialValue;                 // Initial accumulator value
            var assembler = new Z80AssemblerBuilder();
            assembler.ADDA(valueToAdd);
            var bus = new TestBusBuilder().Add(assembler.Build()).BuildBus();
            _processor.ConnectToDataBus(bus);
            await _processor.RunOnce();
            _processor.Registers.Main.F.HasFlag(Z80Flags.C).Should().Be(carryExpected);
        }

        [Theory]
        [InlineData(0x3e,0x22, true)]           // 0x3e = 0011 1110
                                                // 0x22 = 0010 0010
                                                // (+)  = 0110 0000     = 0x60
                                                // c    =   ** ***
        [InlineData(0x39, 0x48, true)]         
        [InlineData(0xff, 1, true)]
        [InlineData(0b01010101, 0b10101010,false)]
        public async Task ADDAN_Should_Set_HalfCarry_When_Intermediate_Carry_Is_Observed(byte initialValue, byte valueToAdd, bool halfCarryExpected)
        {
            _processor.Registers.Main.A = initialValue;                 // Initial accumulator value
            var assembler = new Z80AssemblerBuilder();
            assembler.ADDA(valueToAdd);
            var bus = new TestBusBuilder().Add(assembler.Build()).BuildBus();
            _processor.ConnectToDataBus(bus);
            await _processor.RunOnce();
            _processor.Registers.Main.F.HasFlag(Z80Flags.H).Should().Be(halfCarryExpected);
        }

    }
}
