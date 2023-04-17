using SantMarti.Z80.Assembler;

namespace SantMarti.Z80.Tests.Extensions;

static class Z80ProcessorTestExtensions
{
    public static void SetupWithProgram(this Z80Processor processor, TestTickHandler testHandler, Z80AssemblerBuilder asm, ushort startAddress)
    {
        ArgumentNullException.ThrowIfNull(testHandler, nameof(testHandler));
        var program = asm.Build();
        testHandler.OnMemoryRead(startAddress, program);
        processor.Registers.PC = startAddress;
    }

    
}