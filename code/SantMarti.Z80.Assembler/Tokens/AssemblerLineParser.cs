using SantMarti.Z80.Assembler.Encoders;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Tokens;

public class AssemblerLineParser
{
    
    public TokenizedLine Parse(string asm)
    {
        var tokenizedLine = new TokenizedLine(asm);
        
        var semicolon = asm.IndexOf(';');
        var comment = "";
        if (semicolon != -1)
        {
            comment = asm.Substring(semicolon);
            asm = asm.Substring(0, semicolon);
        }
        
        var trimmed = asm.ToUpperInvariant().Trim();
        var space = trimmed.IndexOf(' ');
        var keyword = space != -1 ? trimmed.Substring(0, space) : trimmed;
        tokenizedLine.AddToken(new Opcode(keyword));
        var restLine = space != -1 ? trimmed.Substring(space + 1) : "";
        var tokens = ParseRestLine(restLine);
        tokenizedLine.AddRange(tokens);
        if (!string.IsNullOrEmpty(comment))
        {   
            tokenizedLine.AddToken(new Comment(comment));
        }
        return tokenizedLine;
    }

    private IEnumerable<BaseToken> ParseRestLine(string restLine)
    {
        var operands =  restLine.Split(',');
        foreach (var operand in operands)
        {
            var trimmedOperand = operand.Trim();
            yield return AnyParser.ParseToken(trimmedOperand);
        }
    }
}