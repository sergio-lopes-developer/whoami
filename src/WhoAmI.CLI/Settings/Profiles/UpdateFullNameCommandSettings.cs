using System.ComponentModel;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Settings.Profiles;

public sealed class UpdateFullNameCommandSettings : CommandSettings {
    [CommandOption("--id|-i")]
    [Description("Profile ID")]
    public Guid Id { get; init; } = Guid.Empty;

    [CommandOption("--first-name|-f")]
    [Description("First name")]
    public string FirstName { get; init; } = string.Empty;

    [CommandOption("--last-name|-l")]
    [Description("Last name")]
    public string LastName { get; init; } = string.Empty;
}
