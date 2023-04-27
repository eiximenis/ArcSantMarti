using Microsoft.VisualBasic.CompilerServices;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Tests.Extensions;

namespace SantMarti.Z80.Tests.Instructions;

public class InstructionTestsBase
{
    protected Z80Processor Processor { get; }
    protected TestTickHandler TickHandler { get; }
    protected ushort StartProgramAddress { get; }
    

    protected InstructionTestsBase(ushort startProgramAddress)
    {
        Processor = new Z80Processor();
        TickHandler = new TestTickHandler(Processor);
        StartProgramAddress = startProgramAddress;
    }
    
    protected InstructionTestsBase() : this(10) {}
    
    protected void SetupProcessorWithProgram(Z80AssemblerBuilder asm) 
        => Processor.SetupWithProgram(TickHandler, asm, StartProgramAddress);
    
}