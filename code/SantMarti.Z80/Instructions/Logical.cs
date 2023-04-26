using System.Text.RegularExpressions;
using SantMarti.Z80.Extensions;

namespace SantMarti.Z80.Instructions;

public class Logical
{
    /// <summary>
    ///  AND r: Logically AND register r against A, result in A 
    /// </summary>
    public static void AND_R(Instruction instruction, Z80Processor processor)
    {
        var opcode = instruction.Opcode;
        var source = opcode & 0b00_000_111;
        var value = processor.GetByteRegisterMask(source);
        ref var registers = ref processor.Registers.Main;
        registers.A = Z80Alu.And8(ref registers, registers.A, value);
    }
    

    /// <summary>
    /// AND (HL): Logically AND value pointed by HL against A, result in A
    /// </summary>
    public static void AND_HLRef(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var address = registers.HL;
        var data = processor.MemoryRead(address);
        registers.A = Z80Alu.And8(ref registers, registers.A, data);
    }

    /// <summary>
    /// AND n: Logically AND n against A, result in A
    /// </summary>
    public static void AND_N(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var data = processor.MemoryRead();
        registers.A = Z80Alu.And8(ref registers, registers.A, data);
    }

    public static void AND_IXH(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var data =processor.Registers.IX.HiByte();
        registers.A = Z80Alu.And8(ref registers, registers.A, data);
    }
}