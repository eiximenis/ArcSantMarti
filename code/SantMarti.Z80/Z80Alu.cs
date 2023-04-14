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
    public static byte ByteAdd(ref Z80GenericRegisters registers, byte first, byte second, byte cflag = 0)
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
        registers.SetFlagIf(Z80Flags.Sign, byteResult & 0x80);
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
    public static byte ByteAddNoSetCarry(ref Z80GenericRegisters registers, byte first, byte second)
    {        
        int result =first + second;
        int no_carry_sum = first ^ second;
        int carry_into = result ^ no_carry_sum;
        int half_carry = carry_into & 0x10;
        var byteResult = (byte)result;
        var overflow = ((no_carry_sum & 0x80) == 0) && (((byteResult & 0x80) ^ (first & 0x80)) != 0);
        registers.ClearFlag(Z80Flags.Substract);
        registers.SetFlagIf(Z80Flags.Zero, byteResult == 0);
        registers.SetFlagIf(Z80Flags.HalfCarry, half_carry);
        registers.SetFlagIf(Z80Flags.ParityOrOverflow, overflow);
        registers.SetFlagIf(Z80Flags.Sign, byteResult & 0x80);
        registers.CopyF3F5FlagsFrom(byteResult);
        return byteResult;
    }
    
}