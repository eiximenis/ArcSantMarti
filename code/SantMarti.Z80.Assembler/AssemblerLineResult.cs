using SantMarti.Z80.Assembler.Tokens;

namespace SantMarti.Z80.Assembler;

public class AssemblerLineResult
{
    public byte[]? Bytes { get; }
    public string ErrorMessage { get; } = "";
    
    public BaseToken? OffendingToken { get; }
    
    private AssemblerLineResult(byte[]? bytes, string errorMessage, BaseToken? offending)
    {
        Bytes = bytes;
        ErrorMessage = errorMessage;
        OffendingToken = offending;
    }

    public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasResult => Bytes is not null;
    
    public static AssemblerLineResult Success(params byte[] bytes)
    {
        return new AssemblerLineResult(bytes, "", null);
    }

    public static AssemblerLineResult Error(string error, BaseToken? offendingToken = null)
    {
        return new AssemblerLineResult(null, error, offendingToken);
    }

}