using System.Text;

namespace SantMarti.Tap.Tzx;

public class TzxHeader
{
    public byte MinorVersion { get; }
    public byte MajorVersion { get; }

    public const string MARKER = "ZXTape!";
    public const byte END_OF_TEXT_MARKER = 0x1A;

    // TZX header length is 10 bytes (byte[7] for Marker, byte for EndOfText, 2 bytes for version)
    public int Length => 10;

    public TzxHeader(byte minorVersion, byte majorVersion)
    {
        MajorVersion = majorVersion;
        MinorVersion = minorVersion;
    }
        
    
    public static TzxHeader FromBytes(ReadOnlySpan<byte> data)
    {
        var span = data;
        var fileMarker = Encoding.ASCII.GetString(span.Slice(0, 7));
        if (fileMarker != MARKER)
        {
            throw new InvalidOperationException($"Invalid file marker: {fileMarker}");
        }
        var endOfText = span[7];
        if (endOfText != END_OF_TEXT_MARKER)
        {
            throw  new InvalidDataException($"Invalid end of text marker: {endOfText} (expected {END_OF_TEXT_MARKER})");
        }
        var header = new TzxHeader(span[8], span[9]);
        return header;
    }
}