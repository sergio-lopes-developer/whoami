using System.ComponentModel;
using System.Reflection;
using Spectre.Console.Cli;
using WhoAmI.CLI.Output;

namespace WhoAmI.CLI.Commands;

[Description("Display the CLI version")]
public sealed class VersionCommand : Command {
    protected override int Execute(
        CommandContext context,
        CancellationToken cancellationToken
    ) {
        CliMessage.ShowInfo($"WhoAmI CLI - v{_cliVersion}");
        return CliExit.Success();
    }

    private static readonly string _cliVersion =
        typeof(VersionCommand)
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "unknown";
}
