using SantMarti.Tap.Extensions;

namespace SantMarti.Tap;

public class TapHeader
{
    public TapBlockType Type { get; }
    public string FileName  { get; }
    public ushort DataLength { get; }
    public ushort Param1 { get; }
    public ushort Param2 { get; }

    // TapHeader size is 17 bytes (1 type, 10 filename, 2 data length, 2 param1, 2 param2)
    public const int Size = 17;

    public TapHeader(TapBlockType type, string fileName, ushort dataLength, ushort param1, ushort param2)
    {
        Type = type;
        FileName = fileName;
        DataLength = dataLength;
        Param1 = param1;
        Param2 = param2;
    }

    public static TapHeader FromBytes(ReadOnlySpan<byte> span)
    {
        var type = span[0];
        if (type > 3)  { throw new InvalidDataException($"Invalid block type: {type}"); } 
        var fileName = span.GetString(1, 10);
        var dataLen = span.GetDword(11);
        var param1 = span.GetDword(13);
        var param2 = span.GetDword(15);
        return new TapHeader((TapBlockType)type, fileName, dataLen, param1, param2);
    }
}