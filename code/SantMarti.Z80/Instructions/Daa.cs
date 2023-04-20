namespace SantMarti.Z80.Instructions;

/// <summary>
/// DAA: Decimal Adjust Accumulator
/// Table from http://www.z80.info/z80syntx.htm#DAA
///--------------------------------------------------------------------------------
///|           | C Flag  | HEX value in | H Flag | HEX value in | Number  | C flag|
///| Operation | Before  | upper digit  | Before | lower digit  | added   | After |
///|           | DAA     | (bit 7-4)    | DAA    | (bit 3-0)    | to byte | DAA   |
///|------------------------------------------------------------------------------|
///|           |    0    |     0-9      |   0    |     0-9      |   00    |   0   | 1
///|   ADD     |    0    |     0-8      |   0    |     A-F      |   06    |   0   | 2
///|           |    0    |     0-9      |   1    |     0-3      |   06    |   0   | 8
///|   ADC     |    0    |     A-F      |   0    |     0-9      |   60    |   1   | 3
///|           |    0    |     9-F      |   0    |     A-F      |   66    |   1   | 4
///|   INC     |    0    |     A-F      |   1    |     0-3      |   66    |   1   | 9
///|           |    1    |     0-2      |   0    |     0-9      |   60    |   1   | 5
///|   N=0     |    1    |     0-2      |   0    |     A-F      |   66    |   1   | 6
///|           |    1    |     0-3      |   1    |     0-3      |   66    |   1   | 7
///|------------------------------------------------------------------------------|
///|   N=1     |         |              |        |              |         |       | 
///|   SUB     |    0    |     0-9      |   0    |     0-9      |   00    |   0   | 10
///|   SBC     |    0    |     0-8      |   1    |     6-F      |   FA    |   0   | 11
///|   DEC     |    1    |     7-F      |   0    |     0-9      |   A0    |   1   | 12
///|   NEG     |    1    |     6-F      |   1    |     6-F      |   9A    |   1   | 13
///|------------------------------------------------------------------------------|
///
/// Of course DAA is unaware of the instruction that had run before it. For that
/// relies on the N flag. For ADD, ADC, INC then N = 0. For SUB, SBC, DEC, NEG
/// N flag is 1

public static class Daa
{
    /// </summary>
    /// <param name="instruction"></param>
    /// <param name="processor"></param>
    public static void DAA(Instruction instruction, Z80Processor processor)
    {
        var n = processor.Registers.Main.HasFlag(Z80Flags.Substract);
        var c = processor.Registers.Main.HasFlag(Z80Flags.Carry);
        var h = processor.Registers.Main.HasFlag(Z80Flags.HalfCarry);
        var upperDigit = (byte)processor.Registers.Main.A >> 4;
        var lowerDigit = (byte)processor.Registers.Main.A & 0x0F;

        var (processed, setc, add) = (n, c, h, upperDigit, lowerDigit) switch
        {
            (false, false, false, >=0 and <=9, >=0 and <=9) => (true, false, 0), 
            (false, false, false, >=0 and <=8, >=0xa and <=0xf) => (true, false, 6),
            (false, false, false, >=0xa and <=0xf, >=0 and <=9) => (true, true, 0x60),
            (false, false, false, >=9 and <=0xf, >=0xa and <=0xf) => (true, true, 0x66),
            (false, true, false,  >=0 and <=2, >=0 and <=9) => (true, true, 0x60),
            (false, true, false,  >=0 and <=2, >=0xa and <=0xf) => (true, true, 0x66),
            (false, true, true, >=0 and <=3, >=0 and <=3) => (true, true, 0x66),
            (false, false, true, >=0 and <=9, >=0 and <=3) => (true, false,6),
            (false, false, true, >=0xa and <=0xf, >=0 and <=3) => (true, true,0x66),
            //
            (true, false, false, >=0 and <=9, >=0 and <=9) => (true, false,0),
            (true, false, true, >=0 and <=8, >=6 and <=0xf) => (true, false,0xfa),
            (true, true, false, >=7 and <=0xf, >=0 and <=9) => (true, true,0xa0),
            (true, true, true, >=6 and <=0xf, >=6 and <=0xf) => (true, true,0x9a),
            _ => (false, false, 0)          // What is supposed to happen here? We will just do nothing (NOP)
        };

        ref var regs = ref processor.Registers.Main;
        if (processed)
        {
            var prevA = regs.A;
            regs.A += (byte)add;
            regs.SetFlagIf(Z80Flags.Carry, setc);
            regs.SetFlagIf(Z80Flags.HalfCarry, (prevA & 0x8 ^ regs.A & 0x8) != 0);
            regs.SetFlagIf(Z80Flags.Zero, regs.A == 0);
            regs.SetFlagIf(Z80Flags.Sign, (regs.A & 0x80) != 0);
            regs.CopyF3F5FlagsFrom(regs.A);
        }
    }
}