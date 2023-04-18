namespace SantMarti.Z80.Extensions;

static class UshortExtensions
{
    public static byte HiByte(this ushort value) => (byte)(value >> 8);
    public static byte LoByte(this ushort value) => (byte)(value & 0xFF); 
}