namespace SantMarti.Z80.Assembler.Tokens;

public class NumericValue : BaseToken
{
    public NumericValue(string str, int value, bool isByte ) : base(str, TokenType.Number)
    {
        Value = value;
        IsByte = isByte;
    }
    public bool IsByte { get; }
    public bool IsWord => !IsByte;
    public int Value { get; }
    public byte AsByte() => (byte)Value;
    
    
    public byte HiByte() => (byte)(Value >> 8);
    public byte LoByte() => (byte)(Value & 0xFF);

    public byte[] AsLoHiBytes()
    {
        return new[] {LoByte(), HiByte()};
    }
}