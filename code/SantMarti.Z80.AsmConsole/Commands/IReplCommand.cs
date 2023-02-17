using SantMarti.Z80.AsmConsole.Context;

namespace SantMarti.Z80.AsmConsole.Commands;

interface IReplCommand
{
    public string Name { get; }
    public Task<ExecCodes> Run(ReplContext context, string[] args);
}