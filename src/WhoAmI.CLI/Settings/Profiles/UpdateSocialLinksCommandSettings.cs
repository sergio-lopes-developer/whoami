using System.ComponentModel;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Settings.Profiles;

public sealed class UpdateSocialLinksCommandSettings : CommandSettings {
    [CommandOption("--id|-i")]
    [Description("Profile ID")]
    public Guid Id { get; init; } = Guid.Empty;

    [CommandOption("--linkedin|-n")]
    [Description("LinkedIn profile URL")]
    public string LinkedIn { get; init; } = string.Empty;

    [CommandOption("--github|-g")]
    [Description("GitHub profile URL")]
    public string GitHub { get; init; } = string.Empty;
}
