using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Extensions;
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
        private readonly TestTickHandler _testTickHandler;
        public LDTests()
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
        [Theory()]
        [InlineData("B", "L")]
        [InlineData("L", "B")]
        [InlineData("A", "D")]
        [InlineData("D", "A")]
        [InlineData("C", "H")]
        [InlineData("H", "C")]
        public async Task LDRR_Should_Load_Destination_Into_Source(string destination, string source)
        {
            _processor.Registers.SetByteRegisterByName(destination, 0x0A);
            _processor.Registers.SetByteRegisterByName(source, 0x01);
            var assembler = new Z80AssemblerBuilder();
            assembler.LD_RR(destination, source);
            SetupProcessorWithProgram(assembler);
            await _processor.RunOnce();
            _processor.Registers.GetByteRegisterByName(destination).Should().Be(0x01);
        }
    }
}
