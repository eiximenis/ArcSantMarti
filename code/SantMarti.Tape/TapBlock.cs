using System.Buffers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using SantMarti.Tap.Extensions;

namespace SantMarti.Tap;

public enum TapBlockType
{
    Program = 0,
    NumberArray = 1,
    CharacterArray = 2,
    Code = 3,
}

public class TapBlock
{
    public ushort DataLength { get; }
    public TapHeader? Header { get; }
    public byte[] Data { get;  }

    public TapBlock(ushort length, TapHeader? header, byte[] data)
    {
        DataLength = length;
        Header = header;
        Data = data;
    }
    
    public static TapBlock HeaderOnlyBlockFromBytes(ReadOnlySpan<byte> span)
    {
        var header = TapHeader.FromBytes(span);
        return new TapBlock(0, header, Array.Empty<byte>());
    }

    public static TapBlock RawBlockFromBytes(ReadOnlySpan<byte> span, ushort len)
    {
        return new TapBlock(len, null, span.Slice(0, len).ToArray());
    }
}



