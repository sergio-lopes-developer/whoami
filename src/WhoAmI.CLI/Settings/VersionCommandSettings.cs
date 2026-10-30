using System.ComponentModel;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Settings;

public sealed class VersionCommandSettings : CommandSettings {
    [CommandOption("--verbose|-v")]
    [Description("Displays additional version information")]
    public bool Verbose { get; init; } = false;
}
