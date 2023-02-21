namespace SantMarti.Z80.Assembler.Tokens;

public class RegisterReference : BaseToken
{
    public bool IsByteRegister { get; }
    public bool IsWordRegister => !IsByteRegister;
    public bool IsIndex { get; }
    

    public RegisterReference(string str, bool isByteRegister, bool isIndex) : base(str, TokenType.Register)
    {
        IsByteRegister = isByteRegister;
        IsIndex = isIndex;
    }
}