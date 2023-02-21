namespace SantMarti.Z80.Assembler.Tokens;

public class Comment : BaseToken
{
    public Comment(string comment) : base(comment, TokenType.Comment)
    {
    }
}