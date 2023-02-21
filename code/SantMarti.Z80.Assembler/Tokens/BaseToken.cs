namespace SantMarti.Z80.Assembler.Tokens;

public enum TokenType
{
    Opcode,             // An Opcode like LD or ADD
    Number,             // A number like $10 or 10
    Register,           // A register like A or BC
    Comment,            // A comment like ; This is a comment
    Displacement,       // A displacement like (IX + 10)
    MemoryReference,    // A memory reference like ($10) or (10)
    Unknown             // An unknown token
}

public class BaseToken
{
    public string StrValue { get; }
    public TokenType Type { get; }

    public BaseToken(string str, TokenType type)
    {
        StrValue = str;
        Type = type;
    }
}