namespace SantMarti.Z80.Instructions;

public class Increment
{    public static void INC_A(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.A = Z80Alu.ByteAddNoSetCarry(ref registers, registers.A, 1);
    }
    public static void INC_B(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.B = Z80Alu.ByteAddNoSetCarry(ref registers, registers.B, 1);
    }
    public static void INC_C(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.C = Z80Alu.ByteAddNoSetCarry(ref registers, registers.C, 1);
    }
    public static void INC_D(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.D = Z80Alu.ByteAddNoSetCarry(ref registers, registers.D, 1);
    }
    
    public static void INC_E(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.E = Z80Alu.ByteAddNoSetCarry(ref registers, registers.E, 1);
    }
    
    public static void INC_H(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.H = Z80Alu.ByteAddNoSetCarry(ref registers, registers.H, 1);
    }
    
    public static void INC_L(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.L = Z80Alu.ByteAddNoSetCarry(ref registers, registers.L, 1);
    }

    

    
    
    
    
    
    

}