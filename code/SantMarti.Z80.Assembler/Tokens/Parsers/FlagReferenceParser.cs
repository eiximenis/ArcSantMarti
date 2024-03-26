using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80.Assembler.Tokens.Parsers;
public class FlagReferenceParser
{
    public static TokenParseResult<FlagReference> TryGetFlagReference(string operand)
    {
        var upperOp = operand.ToUpperInvariant();

        if (string.Equals(upperOp, "PE", StringComparison.InvariantCultureIgnoreCase))
        {
            return TokenParseResult<FlagReference>.Success(new FlagReference(upperOp, Z80ReferencedFlag.ParityOrOverflow, isSet: true));
        }

        if (string.Equals(upperOp, "PO", StringComparison.InvariantCultureIgnoreCase))
        {
            return TokenParseResult<FlagReference>.Success(new FlagReference(upperOp, Z80ReferencedFlag.ParityOrOverflow, isSet: false));
        }
            
        return TokenParseResult<FlagReference>.Error($"Value '{operand}' can't be parsed as a Flag Reference");
    }
}
