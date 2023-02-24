using Microsoft.Extensions.Configuration;
using SantMarti.Z80.AsmConsole;

Console.Clear();
var repl = new Repl();
var config = new ConfigurationBuilder()
    .AddJsonFile("z80asm.json", optional: true)
    .AddEnvironmentVariables().Build();

var paths = config["Loader:InputPaths"].Split(";");
foreach (var path in paths)
{
    repl.Context.Loader.AddInputPath(path);
}

await repl.Start();