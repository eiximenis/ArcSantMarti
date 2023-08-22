using System.Collections;
using System.Runtime.InteropServices;

namespace SantMarti.Z80.Assembler.Tokens;

public class TokenizedLine : IEnumerable<BaseToken>
{
    private readonly List<BaseToken> _tokens = new();
    public string Line { get; }

    public IEnumerable<BaseToken> Tokens => _tokens;

    public ReadOnlySpan<BaseToken> Operands => CollectionsMarshal.AsSpan(_tokens).Slice(1);

    internal void AddToken(BaseToken token)
    {
        _tokens.Add(token);
    }
    internal void AddRange(IEnumerable<BaseToken> tokens) => _tokens.AddRange(tokens);

    public TokenizedLine(string line)
    {
        Line = line;
    }

    public IEnumerator<BaseToken> GetEnumerator() => _tokens.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Opcode? GetOpcode() => _tokens.SingleOrDefault(t => t is Opcode) as Opcode;
}