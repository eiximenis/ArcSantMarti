using System.Reflection.Emit;
using System.Text.Json;

namespace SantMarti.Z80.Instructions;

static class Jump
{
    
    /// <summary>
    /// JP nn: Jumps to address nn
    /// Typical way to do it would be to set PC = nn, but this is not what really happens.
    /// Instead:
    ///   1. It sets WZ to the value of nn
    ///   2. Writes WZ to address bus, so next memory read (opcode fetch) will be from nn --> THIS IS THE JUMP
    ///   3. Sets PC to (WZ + 1), so next instruction after the jump will be fetched the usual way using PC
    /// </summary>
    public static void JP_NN(Instruction instruction, Z80Processor processor)
    {
        processor.Registers.Z = processor.MemoryRead();
        processor.Registers.W = processor.MemoryRead();
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

    /// <summary>
    /// DJNZ Decreases B and jumps to address nn if B != 0
    /// </summary>
    public static void DJNZ(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        processor.OnTick();                         // Extra TState inserted
        var offset = processor.MemoryRead();
        registers.B--;
        if (registers.B != 0)
        {
            // 5 extra TStates added if jump is taken
            processor.OnTick(5);
            // offset is a signed byte (negative if bit 7 is set)
            if ((offset & BitConstants.MSB) != 0)
            {
                var offsetTwoComplement = (byte)(~offset + 1);
                processor.Registers.WZ = (ushort)(processor.Registers.PC - offsetTwoComplement);
            }
            else
            {
                processor.Registers.WZ = (ushort)(processor.Registers.PC +  offset);
            }
            processor.Registers.PC = (ushort)(processor.Registers.WZ + 1);
            processor.OnNextFetchUseWZ();            
        }
    }
}