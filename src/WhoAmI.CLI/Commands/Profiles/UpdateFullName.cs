using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.CLI.Commands.Core;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Commands.Profiles;

[Description("Update the profile's name")]
public sealed class UpdateFullName(
    ICommandDispatcher dispatcher,
    ILogger<UpdateFullName> logger
) : CommandBase<UpdateFullName, UpdateFullNameCommandSettings>(logger) {
    protected override async Task<int> ExecuteCommandAsync(
        CommandContext context,
        UpdateFullNameCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) =>
        await ExecuteInternalAsync(context, commandSettings, cancellationToken);

    internal async Task<int> ExecuteInternalAsync(
        CommandContext context,
        UpdateFullNameCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) {
        var command = new UpdateFullNameCommand(
            commandSettings.Id,
            commandSettings.FirstName,
            commandSettings.LastName
        );

        var result = await dispatcher.Send(command, cancellationToken);

        return ShowCommandResult(result, "Name successfully updated.");
    }
}
