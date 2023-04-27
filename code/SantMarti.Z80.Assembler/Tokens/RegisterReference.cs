using System.Reflection;
using System.Security.Cryptography;

namespace SantMarti.Z80.Assembler.Tokens;


public enum RegisterType  {
    GenericByte,    // A B C D E H L
    GenericWord,    // AF BC DE HL
    IndexWord,      // IX IY
    OtherWord,      // PC SP
    IndexByte,      // IXH IXL IYH IYL,
    OtherByte       // I R
}



public class RegisterReference : BaseToken
{
    public bool IsAccumulator => StrValue == "A";
    public RegisterType RegisterType { get; }

    public RegisterReference(string str, RegisterType type) : base(str, TokenType.Register)
    {
        RegisterType = type;
    }
    
    
}

    
