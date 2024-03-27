using SantMarti.Z80.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Instructions;
public static class Bitwise
{
    /// <summary>
    /// BIT n,r: Checks if n-th is enabled in register r (leaves result in Z flag)
    /// </summary>
    public static void Bit_R(Instruction instruction, Z80Processor processor)
    {
        var idx = (byte)((instruction.Opcode & 0b00111000) >> 3);
        var regValue = processor.GetByteRegisterMask(instruction.Opcode & 0b00000111);
        var testSet = BitCheck(regValue, idx, processor);

        // BIT n,R updates F3 if idx is 3 and bit tested is set
        // BIT n,R updates F5 if idx is 5s and bit tested is set

        processor.Registers.Main.SetFlagIf(Z80Flags.F3, idx == 3 && testSet);
        processor.Registers.Main.SetFlagIf(Z80Flags.F5, idx == 5 && testSet);
    }

    public static void BIT_HLRef (Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var idx = (byte)((instruction.Opcode & 0b00111000) >> 3);
        var value = processor.MemoryRead(registers.HL);
        BitCheck(value, idx, processor);

        // BIT n,(HL) updates F3F5 from high byte (W) of MEMPTR (WZ)!!!!!
        processor.Registers.Main.CopyF3F5FlagsFrom(processor.Registers.W);
    }


    public static void BIT_DispIX(Instruction instruction, Z80Processor processor) => BIT_Displacement(instruction, processor, processor.Registers.IX);
    public static void BIT_DispIY(Instruction instruction, Z80Processor processor) => BIT_Displacement(instruction, processor, processor.Registers.IY);

    private static void BIT_Displacement(Instruction instruction, Z80Processor processor, ushort dispBasee)
    {
        var idx = (byte)((instruction.Opcode & 0b00111000) >> 3);
        var offset = processor.Registers.Z;
        var address = (ushort)(dispBasee + (sbyte)offset);
        var value = processor.MemoryRead(address);
        BitCheck(value, idx, processor);

        // BIT n,(IX|IY + d) updates F3F5 from high byte of IX|IY + d;
        processor.Registers.Main.CopyF3F5FlagsFrom(address.HiByte());
    }

    private static bool BitCheck(byte value, byte idx, Z80Processor processor)
    {
        var mask = (byte)(0x1 << idx);
        ref var registers = ref processor.Registers.Main;

        /* BIT flags affected:
           SF flag Set if idx = 7 and tested bit is set.
           ZF flag Set if the tested bit is reset.
           HF flag Always set.
           PF flag Set just like ZF flag.
           NF flag Always reset.
           CF flag Unchanged.
        */

        bool testedSet = (value & mask) == 1;

        registers.SetFlagIf(Z80Flags.Sign, idx == 7 && testedSet);
        registers.SetFlagIf(Z80Flags.Zero | Z80Flags.ParityOrOverflow, !testedSet);
        registers.SetFlag(Z80Flags.HalfCarry);
        registers.ClearFlag(Z80Flags.Substract);
        processor.OnTick();                             // All BIT instructions add an extra clock cycle

        return testedSet;
    }
}


