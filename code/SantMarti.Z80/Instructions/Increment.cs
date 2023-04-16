namespace SantMarti.Z80.Instructions;

public class Increment
{    public static void INC_A(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.A = Z80Alu.Inc8(ref registers, registers.A);
    }
    public static void INC_B(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.B = Z80Alu.Inc8(ref registers, registers.B);
    }
    public static void INC_C(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.C = Z80Alu.Inc8(ref registers, registers.C);
    }
    public static void INC_D(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.D = Z80Alu.Inc8(ref registers, registers.D);
    }
    
    public static void INC_E(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.E = Z80Alu.Inc8(ref registers, registers.E);
    }
    
    public static void INC_H(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.H = Z80Alu.Inc8(ref registers, registers.H);
    }
    
    public static void INC_L(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        registers.L = Z80Alu.Inc8(ref registers, registers.L);
    }
    

}