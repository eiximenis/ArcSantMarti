using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Tokens;

namespace SantMarti.Z80.AsmConsole.Context;

class AssembledFile
{
    private List<AssembledLine> _code = new();
    public IEnumerable<AssembledLine> Code => _code;
    
    public int LinesCount => _code.Count;
        
    public void Add(TokenizedLine line, AssemblerLineResult asmResult)
    {
        _code.Add(new AssembledLine(line, asmResult));
    }

    public AssembledLine this[int idx] => _code[idx];
}