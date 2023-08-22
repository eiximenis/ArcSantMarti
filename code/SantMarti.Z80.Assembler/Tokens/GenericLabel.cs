namespace SantMarti.Z80.Assembler.Tokens;

public class GenericLabel : BaseToken
{
    public GenericLabel(string str) : base(str, TokenType.Unknown)
    {
    }
}