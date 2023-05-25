namespace SantMarti.Z80.Assembler.Encoders;

public static class NumericEncoder
{
    public static string EncodeHexByte(byte value) => $"${value:X2}";
}