using System.Text;
using Microsoft.Win32.SafeHandles;
using SantMarti.Z80.AsmConsole.Commands;
using SantMarti.Z80.AsmConsole.Context;

namespace SantMarti.Tap;

class LoadTapeCommand : IReplCommand
{
    public const string COMMAND_NAME = "LOADTAPE";
    public string Name => COMMAND_NAME;

    public async Task<ExecCodes> Run(ReplContext context, string[] args)
    {
        var fname = args[0];
        if (string.IsNullOrEmpty(fname))
        {
            Console.WriteLine("Need to specify a file name");
            return ExecCodes.Error;
        }
        var tapeFile =  await context.TryLoadTzxFile(fname);
        if (tapeFile is null)
        {
            Console.WriteLine($"Can't load file {fname}");
            return ExecCodes.Error;
        }
        Console.WriteLine($"Loaded file {fname}");
        return ExecCodes.Ok;
        
    }
}