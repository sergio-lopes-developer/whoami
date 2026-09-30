using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.DeleteProfile;
using WhoAmI.CLI.Commands.Core;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Commands.Profiles;

[Description("Delete a profile")]
public sealed class DeleteProfile(
    ICommandDispatcher dispatcher,
    ILogger<DeleteProfile> logger
) : CommandBase<DeleteProfile, DeleteProfileCommandSettings>(logger) {
    protected override async Task<int> ExecuteCommandAsync(
        CommandContext context,
        DeleteProfileCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) =>
        await ExecuteInternalAsync(context, commandSettings, cancellationToken);

    internal async Task<int> ExecuteInternalAsync(
        CommandContext context,
        DeleteProfileCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) {
        var command = new DeleteProfileCommand(commandSettings.Id);

        var result = await dispatcher.Send(command, cancellationToken);

        return ShowCommandResult(result, "Profile successfully deleted.");
    }
}
