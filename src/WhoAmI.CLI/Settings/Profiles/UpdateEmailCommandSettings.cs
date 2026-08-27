using System.ComponentModel;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Settings.Profiles;

public sealed class UpdateEmailCommandSettings : CommandSettings {
    [CommandOption("--id|-i")]
    [Description("Profile ID")]
    public Guid Id { get; init; } = Guid.Empty;

    [CommandOption("--email|-e")]
    [Description("Email address")]
    public string Email { get; init; } = string.Empty;
}

