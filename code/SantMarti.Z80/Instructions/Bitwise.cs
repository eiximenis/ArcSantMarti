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
        BitCheck(regValue, idx, processor);
    }

    public static void BIT_HLRef (Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var idx = (byte)((instruction.Opcode & 0b00111000) >> 3);
        var value = processor.MemoryRead(registers.HL);
        BitCheck(value, idx, processor);
    }


    public static void BIT_DispIX(Instruction instruction, Z80Processor processor) => BIT_Displacement(instruction, processor, processor.Registers.IX);
    public static void BIT_DispIY(Instruction instruction, Z80Processor processor) => BIT_Displacement(instruction, processor, processor.Registers.IY);

    private static void BIT_Displacement(Instruction instruction, Z80Processor processor, ushort dispBasee)
    {
        var idx = (byte)((instruction.Opcode & 0b00111000) >> 3);
        var offset = processor.MemoryRead();
        var value = processor.MemoryRead((ushort)(dispBasee + (sbyte)offset));
        BitCheck(value, idx, processor);
    }

    private static void BitCheck(byte value, byte idx, Z80Processor processor)
    {
        var mask = (byte)(0x1 << idx);
        ref var registers = ref processor.Registers.Main;

        registers.SetFlagIf(Z80Flags.Zero, (value & mask) == 0);
        registers.SetFlag(Z80Flags.HalfCarry);
        registers.ClearFlag(Z80Flags.Substract);
    }
}


