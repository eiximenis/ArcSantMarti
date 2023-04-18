using FluentAssertions;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Extensions;
using SantMarti.Z80.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace SantMarti.Z80.Tests.Instructions
{
    public class LDTests
    {
        private readonly Z80Processor _processor;
        private readonly TestTickHandler _testTickHandler;
        private const ushort START_ADDRESS = 0x10;
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

        [Theory()]
        [InlineData("B")]
        [InlineData("C")]
        [InlineData("D")]
        [InlineData("E")]
        [InlineData("A")]

        public async Task LD_HRef_R_Should_Write_Memory_Pointed_By_HL(string source)
        {
            var random = new Random();
            var address = random.GetRandomAddress(START_ADDRESS + 10);
            var byteValue = random.GetRandomByte();
            _processor.Registers.SetByteRegisterByName(source, byteValue); 
            _processor.Registers.Main.HL = address;
            var assembler = new Z80AssemblerBuilder();
            assembler.LD("(HL)", source);
            _processor.SetupWithProgram(_testTickHandler, assembler, START_ADDRESS);
            await _processor.RunOnce();
            _testTickHandler.HasAddressBeenWritten(address).Should().BeTrue();
            _testTickHandler.GetMemoryWrittenAt(address).Should().Be(byteValue);
        }
        
        [Theory()]
        [InlineData("H")]
        [InlineData("L")]
        public async Task LD_HRef_H_Or_L_Should_Write_Memory_Pointed_By_HL(string source)
        {
            var random = new Random();
            var address = random.GetRandomAddress(START_ADDRESS + 10);
            _processor.Registers.Main.HL = address;             // Setting HL we are also setting H and L  
            var assembler = new Z80AssemblerBuilder();
            assembler.LD("(HL)", source);
            _processor.SetupWithProgram(_testTickHandler, assembler, START_ADDRESS);
            await _processor.RunOnce();
            _testTickHandler.HasAddressBeenWritten(address).Should().BeTrue();
            _testTickHandler.GetMemoryWrittenAt(address).Should().Be(_processor.Registers.GetByteRegisterByName(source));
        }
        
        [Theory()]
        [InlineData("B")]
        [InlineData("C")]
        [InlineData("D")]
        [InlineData("E")]
        [InlineData("A")]
        public async Task LD_HRef_R_Should_Last_For_7_TStates(string source)
        {
            const int EXPECTED_TICKS = 7;
            var random = new Random();
            var address = random.GetRandomAddress(START_ADDRESS + 10);
            var byteValue = random.GetRandomByte();
            _processor.Registers.SetByteRegisterByName(source, byteValue); 
            _processor.Registers.Main.HL = address;
            var assembler = new Z80AssemblerBuilder();
            assembler.LD("(HL)", source);
            _processor.SetupWithProgram(_testTickHandler, assembler, START_ADDRESS);
            await _processor.RunOnce();
            _testTickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
        }
    }
}
