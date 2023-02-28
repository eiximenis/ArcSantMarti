namespace SantMarti.Z80.Assembler.Tokens;

public class MemoryReference : BaseToken
{
    public ushort Address { get; }
    public RegisterReference? SourceRegister { get; }
    
    public bool IsFixedAddress => SourceRegister is null;
    public MemoryReference(string str, ushort address) : base(str, TokenType.MemoryReference)
    {
        Address = address;
        SourceRegister = null;
    }

    public MemoryReference(string str, RegisterReference register) : base(str, TokenType.MemoryReference)
    {
        Address = 0x0;
        SourceRegister = register;
    }
    
    public string SourceRegisterName => SourceRegister?.StrValue ?? string.Empty;
    
    public byte HiByte() => (byte)(Address >> 8);
    public byte LoByte() => (byte)(Address & 0xFF); 
}