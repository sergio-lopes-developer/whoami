using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.UpdateEmail;
using WhoAmI.CLI.Commands.Core;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Commands.Profiles;

[Description("Update the profile's email")]
public sealed class UpdateEmail(
    ICommandDispatcher dispatcher,
    ILogger<UpdateEmail> logger
) : CommandBase<UpdateEmail, UpdateEmailCommandSettings>(logger) {
    protected override async Task<int> ExecuteCommandAsync(
        CommandContext context,
        UpdateEmailCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) =>
        await ExecuteInternalAsync(context, commandSettings, cancellationToken);

    internal async Task<int> ExecuteInternalAsync(
        CommandContext context,
        UpdateEmailCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) {
        var command = new UpdateEmailCommand(
            commandSettings.Id,
            commandSettings.Email
        );

        var result = await dispatcher.Send(command, cancellationToken);

        return ShowCommandResult(result, "Email successfully updated.");
    }
}
