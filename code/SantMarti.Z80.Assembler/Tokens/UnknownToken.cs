namespace SantMarti.Z80.Assembler.Tokens;

public class UnknownToken : BaseToken
{
    public UnknownToken(string str) : base(str, TokenType.Unknown)
    {
    }
}