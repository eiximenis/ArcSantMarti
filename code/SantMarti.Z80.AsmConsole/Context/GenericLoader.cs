using System.Security.Cryptography.X509Certificates;
using SantMarti.Tap.Tzx;
using SantMarti.Z80.Assembler;
using SantMarti.Z80.Assembler.Tokens;

namespace SantMarti.Z80.AsmConsole.Context;
record AssembledLine(TokenizedLine TokenizedLine, AssemblerLineResult Result);

class GenericLoader
{
    private  readonly List<string> _inputPaths = new() { "." };
    public IEnumerable<string> InputPaths  => _inputPaths;
    
    public void AddInputPath(string path)
    {
        _inputPaths.Add(path);
    }
    
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
    
    public async Task<TzxFile> LoadTzxFile(string path)
    {
        var tzxFile = await TzxLoader.LoadFromFile(path);
        return tzxFile;
    }
}