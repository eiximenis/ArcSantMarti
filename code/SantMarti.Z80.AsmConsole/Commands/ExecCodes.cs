namespace SantMarti.Z80.AsmConsole.Commands;

enum ExecCodes
{
    Ok = 0,
    Error = 1,
    NotFound = 2
}


static class ExecCodesExtensions
{
    public static bool IsOk(this ExecCodes code) => code == ExecCodes.Ok;
    public static bool IsError(this ExecCodes code) => code != ExecCodes.Ok;
} 
