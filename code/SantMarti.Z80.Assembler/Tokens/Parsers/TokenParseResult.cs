namespace SantMarti.Z80.Assembler.Tokens.Parsers;

public class TokenParseResult
{
    public bool HasValue { get; }
    public bool HasError => !HasValue;
    
    public string ErrorMessage { get; }

    protected TokenParseResult(bool hasValue, string errorMessage)
    {
        HasValue = hasValue;
        ErrorMessage = errorMessage;
    }
    
}

public class TokenParseResult<T> : TokenParseResult where T: BaseToken
{
    private TokenParseResult(T parsedToken) : base(true, "")
    {
        ParsedToken = parsedToken;
    }
    private  TokenParseResult(string error) : base(false, error) {}
    public static TokenParseResult<T> Success(T token)
    {
        return new TokenParseResult<T>(token);
    }
    public static TokenParseResult<T> Error(string message)
    {
        return new TokenParseResult<T>(message);
    }
    public T? ParsedToken { get; }
}