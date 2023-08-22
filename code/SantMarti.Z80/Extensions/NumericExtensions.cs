namespace SantMarti.Z80.Extensions;

static class NumericExtensions
{
    public static byte HiByte(this ushort value) => (byte)(value >> 8);
    public static byte LoByte(this ushort value) => (byte)(value & 0xFF); 
    
    public static byte TwoComplement(this byte value) => (byte)(~value + 1);
}