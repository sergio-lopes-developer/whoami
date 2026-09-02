using System.ComponentModel;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Settings.Profiles;

public sealed class GetProfileCommandSettings : CommandSettings {
    [CommandOption("--email|-e")]
    [Description("Email address")]
    public string Email { get; init; } = string.Empty;

    [CommandOption("--hide-email|-m")]
    [Description("Hide the email address")]
    public bool HideEmail { get; init; } = false;

    [CommandOption("--hide-linkedin|-n")]
    [Description("Hide the LinkedIn profile URL")]
    public bool HideLinkedIn { get; init; } = false;

    [CommandOption("--verbose|-v")]
    [Description("Display all available profile information")]
    public bool Verbose { get; init; } = false;
}
