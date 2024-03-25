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

    public static void OR_R(Instruction instruction, Z80Processor processor)
    {        
        var opcode = instruction.Opcode;
        var source = opcode & 0b00_000_111;
        var value = processor.GetByteRegisterMask(source);
        ref var registers = ref processor.Registers.Main;
        registers.A = Z80Alu.Or8(ref registers, registers.A, value);
    }
    public static void XOR_R(Instruction instruction, Z80Processor processor)
    {        
        var opcode = instruction.Opcode;
        var source = opcode & 0b00_000_111;
        var value = processor.GetByteRegisterMask(source);
        ref var registers = ref processor.Registers.Main;
        registers.A = Z80Alu.Xor8(ref registers, registers.A, value);
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
    /// OR (HL): Logically OR value pointed by HL against A, result in A 
    /// </summary>
    public static void OR_HLRef(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var address = registers.HL;
        var data = processor.MemoryRead(address);
        registers.A = Z80Alu.Or8(ref registers, registers.A, data);
    }
    
    /// <summary>
    /// XOR (HL): Logically XOR value pointed by HL against A, result in A 
    /// </summary>
    public static void XOR_HLRef(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var address = registers.HL;
        var data = processor.MemoryRead(address);
        registers.A = Z80Alu.Xor8(ref registers, registers.A, data);
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
        var data = processor.Registers.IXH;
        registers.A = Z80Alu.And8(ref registers, registers.A, data);
    }
    public static void AND_IXL(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var data = processor.Registers.IXL;
        registers.A = Z80Alu.And8(ref registers, registers.A, data);
    }

    public static void AND_IYH(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var data = processor.Registers.IYH;
        registers.A = Z80Alu.And8(ref registers, registers.A, data);
    }
    public static void AND_IYL(Instruction instruction, Z80Processor processor)
    {
        ref var registers = ref processor.Registers.Main;
        var data = processor.Registers.IYL;
        registers.A = Z80Alu.And8(ref registers, registers.A, data);
    }
    
    public static void CP_R (Instruction instruction, Z80Processor processor)
    {
        var opcode = instruction.Opcode;
        var source = opcode & 0b00_111_000;
        var value = processor.GetByteRegisterMask(source);
        ref var registers = ref processor.Registers.Main;
        Z80Alu.Cp8(ref registers, value);
    }

    public static void CP_N(Instruction instruction, Z80Processor processor)
    {
        var value = processor.MemoryRead();
        ref var registers = ref processor.Registers.Main;
        Z80Alu.Cp8(ref registers, value);
    }
}