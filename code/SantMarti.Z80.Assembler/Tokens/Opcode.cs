namespace SantMarti.Z80.Assembler.Tokens;

public class Opcode : BaseToken
{
    public Opcode(string opcode) : base(opcode, TokenType.Opcode)
    {
    }
}