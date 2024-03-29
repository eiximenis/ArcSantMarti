using SantMarti.Z80.AsmConsole.Context;

namespace SantMarti.Z80.AsmConsole.Commands;

class ExitCommand : IReplCommand
{
    public const string COMMAND_NAME = "EXIT";
    public string Name => COMMAND_NAME;

    public Task<ExecCodes> Run(ReplContext context, string[] args)
    {
        context.Finish = true;
        return Task.FromResult(ExecCodes.Ok);
    }
}