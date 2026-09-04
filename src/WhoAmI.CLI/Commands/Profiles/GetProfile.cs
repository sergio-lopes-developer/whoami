using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.CLI.Commands.Core;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Output.Renderers;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Commands.Profiles;

[Description("Display profile information")]
public sealed class GetProfile(
    IQueryDispatcher dispatcher,
    ILogger<GetProfile> logger
) : CommandBase<GetProfile, GetProfileCommandSettings>(logger) {
    protected override async Task<int> ExecuteCommandAsync(
        CommandContext context,
        GetProfileCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) =>
        await ExecuteInternalAsync(context, commandSettings, cancellationToken);

    internal async Task<int> ExecuteInternalAsync(
        CommandContext context,
        GetProfileCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) {
        var query = new GetProfileByEmailQuery(commandSettings.Email);

        var result = await dispatcher.Send(query, cancellationToken);

        if (result.IsFailure) {
            CliMessage.ShowError(
                result.Errors.Select(e => e.Message).ToArray()
            );

            return CliExit.Error();
        }

        InfoRenderer.Render(
            new InfoRenderOptions {
                Title = "PROFILE",
                Items = BuildInfoItems(result.Value, commandSettings)
            }
        );

        return CliExit.Success();
    }

    private static IEnumerable<InfoItem> BuildInfoItems(
        GetProfileByEmailResponse profile,
        GetProfileCommandSettings settings
    ) {
        if (settings.Verbose) {
            yield return new("Id", profile.Id.ToString());
        }

        yield return new("Name", $"{profile.FirstName} {profile.LastName}");

        if (!settings.HideEmail) {
            yield return new("Email", profile.Email);
        }

        yield return new("GitHub", profile.GitHub);

        if (!settings.HideLinkedIn) {
            yield return new("LinkedIn", profile.LinkedIn);
        }
    }
}
