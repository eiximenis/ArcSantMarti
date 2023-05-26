using SantMarti.Z80.Extensions;

namespace SantMarti.Z80;

public static class Z80Alu
{
    /// <summary>
    /// Perform a byte add operation and set the flags accordingly
    /// </summary>
    /// <param name="registers">Z80 registers</param>
    /// <param name="first">First operand</param>
    /// <param name="second"Second operand></param>
    /// <param name="cflag">Carry flag></param>
    /// <returns>Result (bye)</returns>
    public static byte Add8(ref Z80GenericRegisters registers, byte first, byte second, byte cflag = 0)
    {
        int result =first + second + cflag ;
        int no_carry_sum = first ^ (second + cflag);
        int carry_into = result ^ no_carry_sum;
        int half_carry = carry_into & 0x10;
        int carry = carry_into & 0x100;
        var byteResult = (byte)result;
        
        var overflow = ((no_carry_sum & 0x80) == 0) && (((byteResult & 0x80) ^ (first & 0x80)) != 0);
        registers.ClearFlag(Z80Flags.Substract);
        registers.SetFlagIf(Z80Flags.Zero, byteResult == 0);
        registers.SetFlagIf(Z80Flags.HalfCarry, half_carry);
        registers.SetFlagIf(Z80Flags.Carry, carry);
        registers.SetFlagIf(Z80Flags.ParityOrOverflow, overflow);
        registers.SetSignFor(byteResult);
        registers.CopyF3F5FlagsFrom(byteResult);

        return byteResult;
    }
    
    /// <summary>
    /// Perform a byte add operation and set the flags accordingly but do not set the carry flag
    /// </summary>
    /// <param name="registers">Z80 registers</param>
    /// <param name="first">First operand</param>
    /// <param name="second"Second operand></param>
    /// <returns>Result (bye)</returns>
    public static byte Inc8(ref Z80GenericRegisters registers, byte first)
    {        
        int result = first + 1;
        int no_carry_sum = first ^ 1;
        int carry_into = result ^ no_carry_sum;
        int half_carry = carry_into & BitConstants.BIT5;
        var byteResult = (byte)result;

        // For addition, operands with different signs never cause overflow.
        // When adding operands with similar signs and the result contains a different sign, the Overflow Flag is set.
        var overflow = ((no_carry_sum & BitConstants.MSB) == 0) && (((byteResult & BitConstants.MSB) ^ (first & BitConstants.MSB)) != 0);
        registers.ClearFlag(Z80Flags.Substract);
        registers.SetFlagIf(Z80Flags.Zero, byteResult == 0);
        registers.SetSignFor(byteResult);
        registers.SetFlagIf(Z80Flags.HalfCarry, half_carry);
        registers.SetFlagIf(Z80Flags.ParityOrOverflow, overflow);
        registers.CopyF3F5FlagsFrom(byteResult);
        return byteResult;
    }
    
    public static byte Dec8(ref Z80GenericRegisters registers, byte first)
    {        
        int result = first - 1;
        int no_carry_sum = first ^ 1;
        int carry_into = result ^ no_carry_sum;
        int half_carry = carry_into & BitConstants.BIT5;
        var byteResult = (byte)result;

        // For addition, operands with different signs never cause overflow.
        // When adding operands with similar signs and the result contains a different sign, the Overflow Flag is set.
        var overflow = ((no_carry_sum & BitConstants.MSB) == 0) && (((byteResult & BitConstants.MSB) ^ (first & BitConstants.MSB)) != 0);
        registers.SetFlag(Z80Flags.Substract);
        registers.SetFlagIf(Z80Flags.Zero, byteResult == 0);
        registers.SetSignFor(byteResult);
        registers.SetFlagIf(Z80Flags.HalfCarry, half_carry);
        registers.SetFlagIf(Z80Flags.ParityOrOverflow, overflow);
        registers.CopyF3F5FlagsFrom(byteResult);
        return byteResult;
    }

    public static ushort Add16(ref Z80GenericRegisters registers, ushort first, ushort second, byte cflag = 0)
    {
        var loFirst = first.LoByte();
        var hiFirst = first.HiByte();
        var loSecond = second.LoByte();
        var hiSecond = second.HiByte();
        var result = first + second + cflag;
        
        if ((loFirst + loSecond + cflag) > 0xFF) hiSecond++;
        
        registers.SetFlagIf(Z80Flags.HalfCarry, ((hiFirst & 0x0F) + (hiSecond & 0x0F) > 0xF));
        // For Add16 carry is set if there is a carry from bit 16 to (inexistent) bit 17 
        registers.SetFlagIf(Z80Flags.Carry, result > 0xFFFF);
        // Undocumented Flags - from high byte
        registers.CopyF3F5FlagsFrom(((ushort)result).HiByte());
        // Preserves Sign, Zero, and ParityOrOverflow
        return (ushort)result;

    }

    public static byte And8(ref Z80GenericRegisters registers, byte first, byte second)
    {
        var result =  (byte)(first & second);
        registers.ClearFlag(Z80Flags.Substract | Z80Flags.Carry | Z80Flags.HalfCarry);
        registers.CopyF3F5FlagsFrom(result);
        registers.SetParityFor(result);
        registers.SetSignFor(result);
        return result;
    }

    public static byte Or8(ref Z80GenericRegisters registers, byte first, byte second)
    {
        var result =  (byte)(first | second);
        registers.ClearFlag(Z80Flags.Substract | Z80Flags.Carry | Z80Flags.HalfCarry);
        registers.CopyF3F5FlagsFrom(result);
        registers.SetParityFor(result);
        registers.SetSignFor(result);
        return result;
    }
    
    public static byte Xor8(ref Z80GenericRegisters registers, byte first, byte second)
    {
        var result =  (byte)(first ^ second);
        registers.ClearFlag(Z80Flags.Substract | Z80Flags.Carry | Z80Flags.HalfCarry);
        registers.CopyF3F5FlagsFrom(result);
        registers.SetParityFor(result);
        registers.SetSignFor(result);
        return result;
    }
    
}