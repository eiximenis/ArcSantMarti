using System.Reflection;
using System.Security.Cryptography;

namespace SantMarti.Z80.Assembler.Tokens;


public enum RegisterType  {
    GenericByte,    // A B C D E H L
    Word,           // AF BC DE HL
    IndexWord,      // IX IY
    IndexByte,      // IXH IXL IYH IYL
    
    
    
}


public class RegisterReference : BaseToken
{
    public bool IsByteRegister { get; }
    public bool IsWordRegister => !IsByteRegister;
    public bool IsIndex { get; }
    
    public bool IsGeneric { get; }


    public RegisterReference(string str,  bool isByteRegister, bool isIndex) : base(str, TokenType.Register)
    {
        IsByteRegister = isByteRegister;
        IsIndex = isIndex;
        IsGeneric = IsGenericRegister();
    }

    /// <summary>
    /// Returns if register is "generic register". Standard Register are the ones that accept most
    /// operations.
    /// For byte registers, all registers but R and I are generic.
    /// For word registers all registers but PC, IX and IY are generic.
    /// </summary>
    private bool IsGenericRegister()
    {
        return (IsByteRegister, StrValue) switch
        {
            (true, "A" or "B" or "C" or "D" or "E" or "H" or "L") => true,
            (false, "HL" or "BC" or "DE" or "SP") => true,
            _ => false
        };
    }
}
