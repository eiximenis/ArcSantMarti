namespace SantMarti.Z80.Assembler.Tokens;

/// <summary>
/// Encodes tokens like (IY + d) or (IY + d)
/// </summary>
public class Displacement : BaseToken
{
    public byte Value { get; }
    public string Register { get; }

    public bool UseIX { get; }

    public bool UseIY => !UseIX;

    public bool IsNegative { get; }

    public Displacement(string str, string register, short value ) : base(str, TokenType.Displacement)
    {
        Value = (byte)value;
        IsNegative = value < 0;
        Register = register.ToUpperInvariant();
        UseIX = Register == "IX";
    }
}