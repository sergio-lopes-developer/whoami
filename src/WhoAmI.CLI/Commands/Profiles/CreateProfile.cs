using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.CLI.Commands.Core;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Commands.Profiles;

[Description("Create a new profile")]
public sealed class CreateProfile(
    ICommandDispatcher dispatcher,
    ILogger<CreateProfile> logger
) : CommandBase<CreateProfile, CreateProfileCommandSettings>(logger) {
    protected override async Task<int> ExecuteCommandAsync(
        CommandContext context,
        CreateProfileCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) =>
        await ExecuteInternalAsync(context, commandSettings, cancellationToken);

    internal async Task<int> ExecuteInternalAsync(
        CommandContext context,
        CreateProfileCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) {
        var command = new CreateProfileCommand(
            commandSettings.FirstName,
            commandSettings.LastName,
            commandSettings.Email,
            commandSettings.LinkedIn,
            commandSettings.GitHub
        );

        var result = await dispatcher.Send(command, cancellationToken);

        return ShowCommandResult(result, "Profile successfully created.");
    }
}
