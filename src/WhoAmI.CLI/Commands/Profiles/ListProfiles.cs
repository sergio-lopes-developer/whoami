using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Features.Profiles.ListProfiles;
using WhoAmI.CLI.Commands.Core;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Output.Renderers;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Commands.Profiles;

[Description("List all profiles")]
public sealed class ListProfiles(
    IQueryDispatcher dispatcher,
    ILogger<ListProfiles> logger
) : CommandBase<ListProfiles, ListProfilesCommandSettings>(logger) {
    protected override async Task<int> ExecuteCommandAsync(
        CommandContext context,
        ListProfilesCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) =>
        await ExecuteInternalAsync(context, commandSettings, cancellationToken);

    internal async Task<int> ExecuteInternalAsync(
        CommandContext context,
        ListProfilesCommandSettings commandSettings,
        CancellationToken cancellationToken
    ) {
        var query = new ListProfilesQuery();

        var result = await dispatcher.Send(query, cancellationToken);

        if (result.IsFailure) {
            CliMessage.ShowError(
                result.Errors.Select(e => e.Message).ToArray()
            );

            return CliExit.Error();
        }

        if (result.Value.Count == 0) {
            CliMessage.ShowInfo("No profiles were found.");

            return CliExit.Success();
        }

        var listItems = BuildListItems(result.Value);

        ListRenderer.Render(
            new ListRenderOptions {
                Columns = [
                    new("#"),
                    new("Name"),
                    new("Email")
                ],
                Items = listItems
            }
        );

        return CliExit.Success();
    }

    private static IEnumerable<IReadOnlyList<string>> BuildListItems(
        IReadOnlyCollection<ListProfilesResponse> profiles
    ) {
        var index = 1;

        foreach (var profile in profiles) {
            yield return [
                index.ToString(),
                profile.FullName,
                profile.Email
            ];

            index++;
        }
    }
}
