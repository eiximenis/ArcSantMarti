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
    public class LDTests : InstructionTestsBase
    {
        private const ushort START_ADDRESS = 0x10;
        public LDTests() : base(START_ADDRESS) {}
        
        [Theory()]
        [InlineData("B", "L")]
        [InlineData("L", "B")]
        [InlineData("A", "D")]
        [InlineData("D", "A")]
        [InlineData("C", "H")]
        [InlineData("H", "C")]
        public async Task LDRR_Should_Load_Destination_Into_Source(string destination, string source)
        {
            Processor.Registers.SetByteRegisterByName(destination, 0x0A);
            Processor.Registers.SetByteRegisterByName(source, 0x01);
            var assembler = new Z80AssemblerBuilder();
            assembler.LD_RR(destination, source);
            SetupProcessorWithProgram(assembler);
            await Processor.RunOnce();
            Processor.Registers.GetByteRegisterByName(destination).Should().Be(0x01);
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
            Processor.Registers.SetByteRegisterByName(source, byteValue); 
            Processor.Registers.Main.HL = address;
            var assembler = new Z80AssemblerBuilder();
            assembler.LD("(HL)", source);
            SetupProcessorWithProgram(assembler);
            await Processor.RunOnce();
            TickHandler.HasAddressBeenWritten(address).Should().BeTrue();
            TickHandler.GetMemoryWrittenAt(address).Should().Be(byteValue);
        }
        
        [Theory()]
        [InlineData("H")]
        [InlineData("L")]
        public async Task LD_HRef_H_Or_L_Should_Write_Memory_Pointed_By_HL(string source)
        {
            var random = new Random();
            var address = random.GetRandomAddress(START_ADDRESS + 10);
            Processor.Registers.Main.HL = address;             // Setting HL we are also setting H and L  
            var assembler = new Z80AssemblerBuilder();
            assembler.LD("(HL)", source);
            SetupProcessorWithProgram(assembler);
            await Processor.RunOnce();
            TickHandler.HasAddressBeenWritten(address).Should().BeTrue();
            TickHandler.GetMemoryWrittenAt(address).Should().Be(Processor.Registers.GetByteRegisterByName(source));
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
            Processor.Registers.SetByteRegisterByName(source, byteValue); 
            Processor.Registers.Main.HL = address;
            var assembler = new Z80AssemblerBuilder();
            assembler.LD("(HL)", source);
            SetupProcessorWithProgram(assembler);
            await Processor.RunOnce();
            TickHandler.TotalTicks.Should().Be(EXPECTED_TICKS);
        }
        
        [Theory()]
        [InlineData("B")]
        [InlineData("C")]
        [InlineData("D")]
        [InlineData("E")]
        [InlineData("A")]
        public async Task LD_R_HRef_Should_Load_Register_With_Memory_Contents_Pointed_By_HL(string reg)
        {
            var assembler = new Z80AssemblerBuilder();
            assembler.LD(reg, "(HL)");
            var address = (ushort)100;
            var random = new Random();
            var memContent = random.GetRandomByte();
            TickHandler.OnMemoryRead(address, memContent);
            Processor.Registers.Main.HL = address;
            SetupProcessorWithProgram(assembler);
            await Processor.RunOnce();
            Processor.Registers.GetByteRegisterByName(reg).Should().Be(memContent);
        }
    }
}
