using System.ComponentModel;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Settings.Profiles;

public sealed class DeleteProfileCommandSettings : CommandSettings {
    [CommandOption("--id|-i")]
    [Description("Profile ID")]
    public Guid Id { get; init; } = Guid.Empty;
}
