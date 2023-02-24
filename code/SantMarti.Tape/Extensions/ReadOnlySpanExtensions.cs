using System.Buffers;
using System.Text;

namespace SantMarti.Tap.Extensions;

static class ReadOnlySpanExtensions
{
    public static ushort GetDword(this ReadOnlySpan<byte> span,  int offset = 0)
    {
        return (ushort) (span[offset] + (span[offset + 1] << 8));
    }

    public static string GetString(this ReadOnlySpan<byte> span, int offset, int bytesLen)
    {
        return Encoding.ASCII.GetString(span.Slice(offset, bytesLen));
    }
    
}