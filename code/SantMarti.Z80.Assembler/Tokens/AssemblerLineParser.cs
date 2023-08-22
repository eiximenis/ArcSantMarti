using SantMarti.Z80.Assembler.Encoders;
using SantMarti.Z80.Assembler.Tokens.Parsers;

namespace SantMarti.Z80.Assembler.Tokens;

public class AssemblerLineParser
{
    
    public TokenizedLine Parse(string fullLine)
    {
        var tokenizedLine = new TokenizedLine(fullLine);
        fullLine = fullLine.TrimStart();
        var semicolon = fullLine.IndexOf(';');
        (var comment, var asm) = semicolon switch
        {
            0 => (fullLine, string.Empty),  // Line begins with ; so full line it's a comment
            > 0 => (fullLine.Substring(semicolon), fullLine.Substring(0, semicolon)),   // Code and comment
            _ => ("", fullLine) // No comment found, all line is code
        };

        if (!string.IsNullOrEmpty(asm))
        {
            var trimmed = asm.ToUpperInvariant().Trim();
            var tokens = ParseRestLine(trimmed);
            tokenizedLine.AddRange(tokens);
        }

        if (!string.IsNullOrEmpty(comment))
        {   
            tokenizedLine.AddToken(new Comment(comment));
        }
        return tokenizedLine;
    }

    private IEnumerable<string> GetTokens(string value)
    {
        // A token can start with:
        //  - A open parenthesis ( --> Must end with )
        //  - A double quote "  --> Must end with "
        //  - Any other non-whitespace character --> Must end with : or whitespace or comma
        var inToken = false;
        var endTokenCharacter = '\0';
        var idx = 0;
        var startIdx = 0;
        while (idx < value.Length)
        {
            var cchar = value[idx];
            if (!inToken)
            {
                if (!char.IsWhiteSpace(cchar))
                {
                    endTokenCharacter= cchar switch
                    {
                        '(' => ')',
                        '"' => '"',
                        _ => '\0'               // Use '\0' to indicate that the token ends with comma or whitespace
                    };
                    inToken = true;
                    startIdx = idx;
                }
            }
            else
            {
                if (cchar == endTokenCharacter ||
                    (endTokenCharacter == '\0' && (cchar == ',' || char.IsWhiteSpace(cchar))))
                {
                    inToken = false;
                    var endCharacterInToken = endTokenCharacter == ')' || endTokenCharacter == '"';
                    var tokenLength = idx - startIdx;
                    if (endCharacterInToken)
                    {
                        tokenLength++;
                    }
                    yield return value.Substring(startIdx, tokenLength );
                }
            }
            idx++;
        }

        // EOL reached: just return the rest of the line
        if (inToken)
        {
            yield return value.Substring(startIdx);
        }

    }

    private IEnumerable<BaseToken> ParseRestLine(string restLine)
    {
        var lineTokens = GetTokens(restLine);
        var parsersEnabled = ParsersEnabled.FirstValidToken;
                    
        foreach (var token in lineTokens)
        {
            var parsedToken =  AnyParser.ParseToken(token, parsersEnabled);
            parsersEnabled = parsedToken.Type switch
            {
                TokenType.Label => ParsersEnabled.OpcodeOrParameters,
                TokenType.Opcode => ParsersEnabled.ParametersToken,
                _ => ParsersEnabled.ParametersToken
            };
            yield return parsedToken;
        }
    }
}