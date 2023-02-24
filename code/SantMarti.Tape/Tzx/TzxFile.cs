using System.Text;

namespace SantMarti.Tap.Tzx;

public class TzxFile
{
    private readonly byte[] _data;
    private readonly List<TzxBlock> _blocks = new();
    public bool Parsed { get; private set; }
    public bool IsValid { get; private set; }
    public TzxHeader Header { get; private set; }
    public IEnumerable<TzxBlock> Blocks => _blocks;
    public TzxFile(byte[] data)
    {
        _data = data;
    }
    
    public void Parse()
    {
        if (Parsed)
        {
            return;
        }
        Parsed = true;
        var span = _data.AsSpan();
        Header = ReadHeader();
        span = span[Header.Length..];
        ParseBlocks(span);
        IsValid = true;
        
    }

    private void ParseBlocks(ReadOnlySpan<byte> span)
    {
        var remainingBytes = span.Length;
        while (remainingBytes > 0)
        {
            var block = ParseNextBlock(span);
            _blocks.Add(block);
            span = span[(block.Length + 1)..];
            remainingBytes = span.Length;
        }
    }

    private TzxBlock ParseNextBlock(ReadOnlySpan<byte> span)
    {
        var blockType = (TzxBlockType) span[0];
        return blockType switch
        {
            TzxBlockType.StandardSpeedDataBlock => ParseStandardSpeedDataBlock(span),
            TzxBlockType.SelectBlock => ParseSelectBlock(span),
            _ => throw new NotImplementedException()
        };
    }
    

    private TzxStandardSpedDataBlock ParseStandardSpeedDataBlock(ReadOnlySpan<byte> span)
    {
        EnsureIsBlockOfType(TzxBlockType.StandardSpeedDataBlock, span[0]);
        span = span[1..];
        return TzxStandardSpedDataBlock.FromBytes(span);
    }
    
    private TzxSelectDataBlock ParseSelectBlock(ReadOnlySpan<byte> span)
    {
        EnsureIsBlockOfType(TzxBlockType.SelectBlock, span[0]);
        
        /*
        var length = span[1] + (span[2] << 8);
        var count = span[3];
        var block = new TzxSelectDataBlock(length, count);
        span=span[4..];
        for (var idx =0;idx < length; idx++)
        {
            var data = new TzxSelectData();
            data.RelativeOffset = (ushort)(span[0] + (span[1] << 8));
            data.LengthOfText = span[2];
            data.Text = span[3..data.LengthOfText].ToArray();
            block.Select[idx] = data;
            span = span[(data.LengthOfText + 3)..];
        }
        return block;
        */
        return null;
    }


    public TzxHeader ReadHeader()
    {
        var header = TzxHeader.FromBytes(_data.AsSpan());
        return header;
    }

    
    
    private static void EnsureIsBlockOfType(TzxBlockType type, byte blockType)
    {
        if (blockType != (int)type)
        {
            throw new InvalidDataException($"Expected block of type {{type}} and found {blockType}");
        }
    }
    
}