using System.Text;
using SantMarti.Tap.Extensions;

namespace SantMarti.Tap.Tzx;

public enum TzxBlockType : byte
{
    StandardSpeedDataBlock = 0x10,
    TurboSpeedDataBlock = 0x11,
    PureTone = 0x12,
    SequenceOfPulsesOfVariousLengths = 0x13,
    PureDataBlock = 0x14,
    DirectRecordingBlock = 0x15,
    CSWRecordingBlock = 0x18,
    GeneralizedDataBlock = 0x19,
    PauseOrStopTheTapeCommand = 0x20,
    GroupStart = 0x21,
    GroupEnd = 0x22,
    JumpToBlock = 0x23,
    LoopStart = 0x24,
    LoopEnd = 0x25,
    CallSequence = 0x26,
    ReturnFromSequence = 0x27,
    SelectBlock = 0x28,
    StopTheTapeIfIn48KMode = 0x2A,
    SetSignalLevel = 0x2B,
    TextDescription = 0x30,
    MessageBlock = 0x31,
    ArchiveInfo = 0x32,
    HardwareType = 0x33,
    CustomInfoBlock = 0x35,
    GlueBlock = 0x5A
}

public enum TzxBlockTapFlagType : byte
{
    Header = 0,
    Data = 255
}

public abstract class TzxBlock
{
    protected TzxBlock(TzxBlockType type)
    {
        IdByte = type;
    }
    
    public abstract  int Length { get; }
    public TzxBlockType IdByte { get; } 
}

public class TzxStandardSpedDataBlock : TzxBlock
{
    public TapBlock TapData { get; }
    public ushort PauseMs { get; }
    public ushort BlockLength { get; }
    // Full block length is 2 bytes for pause + 2 bytes for data length + real data length
    public override int Length => BlockLength + 2 * sizeof(ushort);       

    public TzxStandardSpedDataBlock(ushort pauseMs, ushort blockLen, TapBlock tapBlock) : base(TzxBlockType.StandardSpeedDataBlock)
    {
        PauseMs = pauseMs;
        BlockLength = blockLen;
        TapData = tapBlock;
    }

    public static TzxStandardSpedDataBlock FromBytes(ReadOnlySpan<byte> data)
    {
        var ms = data.GetDword();
        var len = data.GetDword(2);
        var flagByte = data[4];
        var block = flagByte switch
        {
            (byte)TzxBlockTapFlagType.Header => TapBlock.HeaderOnlyBlockFromBytes(data[5..]),
            (byte)TzxBlockTapFlagType.Data => TapBlock.RawBlockFromBytes(data[5..], (ushort)(len - 2)),
            _ => throw new InvalidOperationException($"Invalid TapData flag byte: {flagByte}")
        };
        return new TzxStandardSpedDataBlock(ms, len, block);
    }

}


public class TzxSelectDataBlock : TzxBlock
{
    public override int Length { get; }
    public TzxSelectData[] Select { get; }

    public TzxSelectDataBlock(int bytesLen, byte count) : base(TzxBlockType.SelectBlock)
    {
        Length = bytesLen;
        Select = new TzxSelectData[count];
    }
    
}

public class TzxSelectData
{
    public ushort RelativeOffset { get; set; }
    public byte LengthOfText { get; set; }
    public byte[] Text { get; set; } = Array.Empty<byte>();
    public string TextString => Encoding.ASCII.GetString(Text);
}