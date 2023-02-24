using System.Reflection.Metadata;
using SantMarti.Z80.AsmConsole.Commands;
using SantMarti.Z80.AsmConsole.Context;

namespace SantMarti.Z80.AsmConsole;

class Repl
{
    public ReplContext Context { get; } = new();
    private readonly ReplParser _parser = new();
    
    public async Task Start()
    {
        while (!Context.Finish)
        {
            var line = Console.ReadLine();
            var tokenized = _parser.Parse(line);
            if (tokenized.Command is null)
            {
                Console.WriteLine($"Unknown command");
            }
            else
            {
                await tokenized.Command.Run(Context, tokenized.Arguments);
            }
        }
    }
}
