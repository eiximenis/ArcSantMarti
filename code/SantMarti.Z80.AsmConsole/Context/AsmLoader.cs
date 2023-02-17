using SantMarti.Z80.Assembler;

namespace SantMarti.Z80.AsmConsole.Context;
record AssembledLine(string Line, IEnumerable<byte> Bytes);

class AssembledFile
{
    private List<AssembledLine> _code = new();
    public IEnumerable<AssembledLine> Code => _code;
    
    public int LinesCount => _code.Count;
        
    public void Add(string line, IEnumerable<byte> bytes)
    {
        _code.Add(new AssembledLine(line, bytes));
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
            var bytes = builder.Asm(line);
            assembledFile.Add(line, bytes);
        }

        return assembledFile;
    }
}