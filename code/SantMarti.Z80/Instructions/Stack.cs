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
    
    public static void POPHL(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        registers.Main.L = processor.MemoryRead(registers.SP);
        registers.SP++;
        processor.OnTick();
        registers.Main.H = processor.MemoryRead(registers.SP);
        registers.SP++;
    }
    
    public static void POPAF(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        registers.Main.F = (Z80Flags)(processor.MemoryRead(registers.SP));
        registers.SP++;
        processor.OnTick();
        registers.Main.A = processor.MemoryRead(registers.SP);
        registers.SP++;
    }
    
    public static void POPDE(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        registers.Main.E = processor.MemoryRead(registers.SP);
        registers.SP++;
        processor.OnTick();
        registers.Main.D = processor.MemoryRead(registers.SP);
        registers.SP++;
    }
    
    public static void POPBC(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        registers.Main.C = processor.MemoryRead(registers.SP);
        registers.SP++;
        processor.OnTick();
        registers.Main.B = processor.MemoryRead(registers.SP);
        registers.SP++;
    }
}