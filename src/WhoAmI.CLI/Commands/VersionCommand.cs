using System.ComponentModel;
using System.Runtime.InteropServices;
using Spectre.Console.Cli;
using WhoAmI.CLI.Hosting;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Settings;

namespace WhoAmI.CLI.Commands;

[Description("Display the CLI version")]
public sealed class VersionCommand : Command<VersionCommandSettings> {
    public override int Execute(
        CommandContext context,
        VersionCommandSettings settings,
        CancellationToken cancellationToken
    ) {
        CliMessage.ShowInfo($"WhoAmI CLI v{CliVersion.Current}");

        if (!settings.Verbose) return CliExit.Success();

        CliMessage.ShowSeparator();
        CliMessage
            .ShowInfo($"Commit:       {CliVersion.Commit}");
        CliMessage
            .ShowInfo($"Environment:  {RuntimeInformation.OSDescription}");
        CliMessage
            .ShowInfo($"Architecture: {RuntimeInformation.OSArchitecture}");
        CliMessage
            .ShowInfo($".NET Runtime: {Environment.Version}");

        return CliExit.Success();
    }
}
