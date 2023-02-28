namespace SantMarti.Z80.Assembler.Tokens;

/// <summary>
/// Encodes tokens like (IY + d) or (IY + d)
/// </summary>
public class Displacement : BaseToken
{
    public short Value { get; }
    public string Register { get; }
    public Displacement(string str, string register, short value ) : base(str, TokenType.Displacement)
    {
        Value = value;
        Register = register;
    }
}