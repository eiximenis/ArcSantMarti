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
    public bool IsByteRegister => RegisterType == RegisterType.IndexByte || RegisterType == RegisterType.GenericByte;
    public bool IsAccumulator => StrValue == "A";
    public bool IsWordRegister => !IsByteRegister;
    public bool IsIndex => RegisterType == RegisterType.IndexByte || RegisterType == RegisterType.IndexWord;
    public RegisterType RegisterType { get; }
    public bool IsGeneric => RegisterType == RegisterType.GenericByte || RegisterType == RegisterType.GenericWord;


    public RegisterReference(string str, RegisterType type) : base(str, TokenType.Register)
    {
        RegisterType = type;
    }
    
    
}

    
