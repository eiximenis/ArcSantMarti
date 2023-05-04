namespace SantMarti.Z80.Tests.Extensions;

static class RandomExtensions
{
    public static ushort GetRandomUint16(this Random random)
    {
        Span<byte> buffer = stackalloc byte[2];
        random.NextBytes(buffer);
        return BitConverter.ToUInt16(buffer);
    }

    public static byte GetRandomByte(this Random random)
    {
        Span<byte>  buffer = stackalloc byte[1];
        random.NextBytes(buffer);
        return buffer[0];
    }

    public static ushort GetRandomAddress(this Random random, ushort from)
    {
        return (ushort)random.Next(from, ushort.MaxValue + 1);
    }


    public static ushort GetRandomUshort(this Random random) => random.GetRandomAddress(0);
}