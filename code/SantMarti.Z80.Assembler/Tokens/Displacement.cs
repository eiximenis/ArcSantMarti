namespace SantMarti.Z80.Assembler.Tokens;

/// <summary>
/// Encodes tokens like (I
/// </summary>
public class Displacement : BaseToken
{
    public NumericValue Number { get; }
    public string Register { get; }
    public Displacement(string str, string register, NumericValue number) : base(str, TokenType.Displacement)
    {
        Number = number;
        Register = register;
    }
}