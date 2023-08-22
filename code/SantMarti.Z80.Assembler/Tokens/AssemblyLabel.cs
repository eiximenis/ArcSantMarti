namespace SantMarti.Z80.Assembler.Tokens;

public class AssemblyLabel : BaseToken
{
    public string Name { get; }
    public ushort Address { get; set; } // Address is filled when loading code into memory
    
    public AssemblyLabel(string name) : base(name, TokenType.Label)
    {
        Name = name;
    }
    
}