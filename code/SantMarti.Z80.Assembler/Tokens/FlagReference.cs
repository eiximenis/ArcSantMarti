namespace SantMarti.Z80.Assembler.Tokens;


public enum Z80ReferencedFlag
{
    ParityOrOverflow = 1
}
public class FlagReference : BaseToken
{
    public Z80ReferencedFlag Flag { get; }
    public bool IsSet { get; }
    public FlagReference (string name, Z80ReferencedFlag flag, bool isSet) : base(name, TokenType.FlagReference)
    {
        Flag = flag;
        IsSet = isSet;
    }
}