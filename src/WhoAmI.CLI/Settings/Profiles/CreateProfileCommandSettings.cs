using System.ComponentModel;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Settings.Profiles;

public sealed class CreateProfileCommandSettings : CommandSettings {
    [CommandOption("--first-name|-f")]
    [Description("First name")]
    public string FirstName { get; init; } = string.Empty;

    [CommandOption("--last-name|-l")]
    [Description("Last name")]
    public string LastName { get; init; } = string.Empty;

    [CommandOption("--email|-e")]
    [Description("Email address")]
    public string Email { get; init; } = string.Empty;

    [CommandOption("--linkedin|-n")]
    [Description("LinkedIn profile URL")]
    public string LinkedIn { get; init; } = string.Empty;

    [CommandOption("--github|-g")]
    [Description("GitHub profile URL")]
    public string GitHub { get; init; } = string.Empty;
}
