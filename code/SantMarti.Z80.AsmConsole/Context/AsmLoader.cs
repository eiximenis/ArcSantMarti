using System.Security.Cryptography.X509Certificates;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Tokens;

namespace SantMarti.Z80.AsmConsole.Context;
record AssembledLine(TokenizedLine TokenizedLine, AssemblerLineResult Result);

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
class AsmLoader
{
    private  readonly List<string> _inputPaths = new() { "." };
    public IEnumerable<string> InputPaths  => _inputPaths;
    
    public async Task<AssembledFile> LoadAsmFile(string path)
    {
        var assembledFile = new AssembledFile();
        var lines = await File.ReadAllLinesAsync(path);
        var builder = new Z80AssemblerBuilder();
        foreach (var line in lines)
        {
            var tokenizedLine = builder.Tokenize(line);
            var asmResult = builder.Asm(line);
            assembledFile.Add(tokenizedLine, asmResult);
        }

        return assembledFile;
    }
}