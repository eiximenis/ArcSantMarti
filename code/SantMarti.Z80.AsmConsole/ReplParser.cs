using SantMarti.Z80.AsmConsole.Commands;

namespace SantMarti.Z80.AsmConsole;

record TokenizedCommand(IReplCommand? Command, string[] Arguments);
class ReplParser
{
    private readonly Dictionary<string, IReplCommand> _commands = new();

    public ReplParser()
    {
        _commands.Add(LoadCommand.NAME, new LoadCommand());
        _commands.Add(ExitCommand.NAME, new ExitCommand());
    }

    public TokenizedCommand Parse(string line)
    {
        var tokens = line.Split(' ').Select(t => t.Trim()).ToArray();
        var commandName = tokens.FirstOrDefault() ?? "";
        var command = _commands.GetValueOrDefault(commandName);
        var args = tokens[1..];
        return new TokenizedCommand(command, args);
    }
}