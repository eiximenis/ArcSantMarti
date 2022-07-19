using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SantMarti.Z80.Tests.Instructions
{
    public class LDTests
    {
        private readonly Z80Processor _processor;
        public LDTests()
        {
            _processor = new Z80Processor();
        }

        [Theory()]
        [InlineData("B", "L")]
        [InlineData("L", "B")]
        [InlineData("A", "D")]
        [InlineData("D", "A")]
        [InlineData("C", "H")]
        [InlineData("H", "C")]
        public async Task LDXY_Should_Load_Destination_Into_Source(string destination, string source)
        {
            _processor.Registers.Main.SetByName(destination, 0x0A);
            _processor.Registers.Main.SetByName(source, 0x01);
            var assembler = new Z80AssemblerBuilder();
            assembler.LD(destination, source);
            var bus = new TestBusBuilder().Add(assembler.Build()).BuildBus();
            _processor.ConnectToDataBus(bus);
            await _processor.RunOnce();
            _processor.Registers.Main.GetByName(destination).Should().Be(0x01);
        }
    }
}
