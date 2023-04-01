namespace SantMarti.Z80.Instructions;

public class Stack
{
    public static void PUSHL(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        // PUSH inserts an extra clock tick to decrement SP
        registers.SP--;
        processor.OnTick();
        processor.MemoryWrite(registers.SP, registers.Main.H);
        registers.SP--;
        processor.MemoryWrite(registers.SP, registers.Main.L);
    }
}