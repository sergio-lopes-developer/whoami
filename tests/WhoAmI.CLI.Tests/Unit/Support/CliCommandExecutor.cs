using Spectre.Console;
using Spectre.Console.Testing;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Tests.Unit.Support;

public static class CliCommandExecutor {
    public static async Task<(
        int ExitCode,
        string ConsoleOutput
    )> Execute<TSettings> (
        TSettings settings,
        Func<CommandContext, TSettings, CancellationToken, Task<int>> execute
    ) where TSettings : CommandSettings {
        var originalConsole = AnsiConsole.Console;
        var console = new TestConsole();

        try {
            AnsiConsole.Console = console;

            var exitCode = await execute(
                CommandContextFactory.Create(),
                settings,
                CancellationToken.None
            );

            return (exitCode, console.Output);
        }
        finally {
            AnsiConsole.Console = originalConsole;
        }
    }
}
