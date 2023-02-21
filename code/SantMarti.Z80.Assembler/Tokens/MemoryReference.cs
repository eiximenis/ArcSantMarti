namespace SantMarti.Z80.Assembler.Tokens;

public class MemoryReference : BaseToken
{
    public int Address { get; }
    public MemoryReference(string str, int address) : base(str, TokenType.MemoryReference)
    {
        Address = address;
    }
}