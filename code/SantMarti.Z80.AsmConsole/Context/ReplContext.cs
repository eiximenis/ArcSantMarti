using SantMarti.Z80.AsmConsole.Commands;

namespace SantMarti.Z80.AsmConsole.Context;

class ReplContext
{

    public AssembledFile? CurrentFile { get; private set; }
    public AsmLoader Loader { get; } = new();
    public bool Finish { get; set; }

    public void SetAssembledFile(AssembledFile assembledFile)
    {
        CurrentFile = assembledFile;
    }
}


static class ReplContextExtensions
{
    public static async Task<AssembledFile?> TryLoadAsmFile(this ReplContext context, string path)
    {
        foreach (var  paths in context.Loader.InputPaths)
        {
            var fullPath = Path.Combine(paths, path);
            if (File.Exists(fullPath))
            { 
                return await context.Loader.LoadAsmFile(fullPath);
            }
        }   
        return null;
    }
}