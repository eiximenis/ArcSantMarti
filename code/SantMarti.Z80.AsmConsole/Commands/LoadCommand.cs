using SantMarti.Z80.AsmConsole.Context;

namespace SantMarti.Z80.AsmConsole.Commands;

class LoadCommand : IReplCommand
{
    public const string NAME = "LOAD";
    public string Name => NAME;
    public async Task<ExecCodes> Run(ReplContext context, string[] args)
    {
        var fname = args[0];
        if (string.IsNullOrEmpty(fname))
        {
            Console.WriteLine("Need to specify a file name");
            return ExecCodes.Error;
        }
        var assembledFile = await context.TryLoadAsmFile(fname);
        if (assembledFile is not null)
        {
            context.SetAssembledFile(assembledFile);
            DumpFirstLines(assembledFile);
            return ExecCodes.Ok;
        }
        
        Console.WriteLine($"Can't load file {fname}");
        return ExecCodes.Error;
    }

    private void DumpFirstLines(AssembledFile assembledFile)
    {
        Console.WriteLine($"CODE COUNT: {assembledFile.LinesCount}");
        var maxCount = Math.Min(assembledFile.LinesCount, 5);
        for (var idx = 0; idx<maxCount; idx++)
        {
            var line = assembledFile[idx];
            DumpLine(idx, line);
        }
    }

    private void DumpLine(int idx, AssembledLine line)
    {
        Console.WriteLine($"{idx}:\t {line.Line} \t {Convert.ToHexString(line.Bytes.ToArray())}");
    }
}