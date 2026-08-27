using System.ComponentModel;
using Spectre.Console.Cli;
using WhoAmI.CLI.Hosting;
using WhoAmI.CLI.Output;

namespace WhoAmI.CLI.Commands;

[Description("Display the CLI version")]
public sealed class VersionCommand : Command {
    protected override int Execute(
        CommandContext context,
        CancellationToken cancellationToken
    ) {
        CliMessage.ShowInfo($"WhoAmI CLI - v{CliVersion.Current}");

        return CliExit.Success();
    }
}
