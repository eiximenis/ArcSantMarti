using SantMarti.Z80.AsmConsole.Context;
using SantMarti.Z80.Assembler.Tokens;

namespace SantMarti.Z80.AsmConsole.Commands;

class LoadCommand : IReplCommand
{
    public const string COMMAND_NAME = "LOAD";
    public string Name => COMMAND_NAME;
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
        var maxCount = Math.Min(assembledFile.LinesCount, 30);
        for (var idx = 0; idx<maxCount; idx++)
        {
            var line = assembledFile[idx];
            DumpLine(idx, line);
        }
    }

    private void DumpLine(int idx, AssembledLine line)
    {
        Console.Write($"{idx}:");
        foreach (var token in line.TokenizedLine)
        {
            var color = token.Type switch
            {
                TokenType.Comment => ConsoleColor.Green,
                TokenType.Number => ConsoleColor.Yellow,
                TokenType.Opcode => ConsoleColor.Blue,
                TokenType.Displacement => ConsoleColor.DarkYellow,
                TokenType.Register => ConsoleColor.Magenta,
                TokenType.MemoryReference => ConsoleColor.Cyan,
                TokenType.FlagReference => ConsoleColor.DarkMagenta,
                _ => ConsoleColor.Red
            };
            var oldcolor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write($"{token.StrValue} ");
            Console.ForegroundColor = oldcolor;
        }

        if (line.Result.HasResult)
        {
            Console.Write($"\t{Convert.ToHexString(line.Result.Bytes!)}");
        }
        else
        {
            Console.WriteLine();
            Console.Write($"\t\t***ERROR: {line.Result.ErrorMessage}");
        }

        Console.WriteLine();

    }
}