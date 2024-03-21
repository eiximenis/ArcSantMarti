using System.Reflection.Emit;
using System.Text.Json;
using SantMarti.Z80.Extensions;

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
            RelativeJump(processor, offset);
        }
    }

    /// <summary>
    /// JR d: Unconditional relative jump (PC = PC + d)
    /// </summary>
    public static void JR_D(Instruction instruction, Z80Processor processor)
    {
        var offset = processor.MemoryRead();
        processor.OnTick(5);
        RelativeJump(processor, offset);
    }

    /// <summary>
    /// JP PE,nn: Jump to nn if P/E (parity/even) is set
    /// </summary>
    public static void JP_PE_NN(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        registers.Z = processor.MemoryRead();
        registers.W = processor.MemoryRead();
        registers.PC = (ushort)(registers.WZ + 1);

        if (processor.Registers.Main.HasFlag(Z80Flags.ParityOrOverflow))
        {
            processor.OnNextFetchUseWZ();
        }
    }

    /// <summary>
    /// CALL nn: Pushes PC to stack and jumps to address nn
    ///  
    public static void CALL_NN(Instruction instruction, Z80Processor processor)
    {
        var registers = processor.Registers;
        registers.Z = processor.MemoryRead();
        registers.W = processor.MemoryRead();
        // extra clock cycle here
        processor.OnTick();
        registers.SP--;
        processor.MemoryWrite(registers.SP, (byte)(registers.PC >> 8));
        registers.SP--;
        processor.MemoryWrite(registers.SP, (byte)(registers.PC & 0xFF));
        processor.Registers.PC = (ushort)(processor.Registers.WZ + 1);
        processor.OnNextFetchUseWZ();
    }

    public static void CALL_CC_NN(Instruction instruction, Z80Processor processor)
    {
        
    }
    


    private static void RelativeJump(Z80Processor processor, byte offset)
    {
        if ((offset & BitConstants.MSB) != 0)
        {
            var offsetTwoComplement = offset.TwoComplement();
            processor.Registers.WZ = (ushort)(processor.Registers.PC - offsetTwoComplement - 2);        // -2 because PC was already incremented twice and jump is relative to opcode addr
        }
        else
        {
            processor.Registers.WZ = (ushort)(processor.Registers.PC +  offset - 2);                    // -2 because PC was already incremented twice and jump is relative to opcode addr
        }
        processor.Registers.PC = (ushort)(processor.Registers.WZ + 1);
        processor.OnNextFetchUseWZ();                
    }

}