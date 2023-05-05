using System.Reflection.Emit;
using System.Text.Json;

namespace SantMarti.Z80.Instructions;

static class Jump
{
    
    public static void JP_NN(Instruction instruction, Z80Processor processor)
    {
        processor.Registers.Z = processor.MemoryRead();
        processor.Registers.W = processor.MemoryRead();
        // JP nn does NOT set the PC to the value of (nn).
        // Instead:
        //   1. It sets WZ to the value of (nn)
        //   2. Writes WZ to address bus, so next memory read (opcode fetch) will be from (nn). <-- THIS IS THE JUMP
        //   3. Sets PC to (WZ + 1), so next instruction after the jump will be fetched the usual way using PC
        processor.Registers.PC = (ushort)(processor.Registers.WZ + 1);
        processor.OnNextFetchUseWZ();
    }
    public static void JP_HL(Instruction instruction, Z80Processor processor)
    {
        processor.Registers.PC = processor.Registers.Main.HL;
    }
    public static void JP_IX(Instruction instruction, Z80Processor processor)
    {
        processor.Registers.PC = processor.Registers.IX;
    }

    public static void JP_IY(Instruction instruction, Z80Processor processor)
    {
        processor.Registers.PC = processor.Registers.IY;
    }

    public static void DZNZ(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        processor.OnTick();                         // Extra TState inserted
        registers.B--;
        var operand = processor.MemoryRead();
        if (registers.B != 0)
        {
            // 5 extra TStates added if jump is taken
            processor.OnTick();   
            processor.OnTick();   
            processor.OnTick();   
            processor.OnTick();   
            processor.OnTick();
        }
        
        
    }
}