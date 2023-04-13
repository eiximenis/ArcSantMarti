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
    
    public static void PUSHAF(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        // PUSH inserts an extra clock tick to decrement SP
        registers.SP--;
        processor.OnTick();
        processor.MemoryWrite(registers.SP, registers.Main.A);
        registers.SP--;
        processor.MemoryWrite(registers.SP, (byte)registers.Main.F);
    }
    
    public static void PUSHDE(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        // PUSH inserts an extra clock tick to decrement SP
        registers.SP--;
        processor.OnTick();
        processor.MemoryWrite(registers.SP, registers.Main.D);
        registers.SP--;
        processor.MemoryWrite(registers.SP, (byte)registers.Main.E);
    }
    
    public static void PUSHBC(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        // PUSH inserts an extra clock tick to decrement SP
        registers.SP--;
        processor.OnTick();
        processor.MemoryWrite(registers.SP, registers.Main.B);
        registers.SP--;
        processor.MemoryWrite(registers.SP, (byte)registers.Main.C);
    }
}