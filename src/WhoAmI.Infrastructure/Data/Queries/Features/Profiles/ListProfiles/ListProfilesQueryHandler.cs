using Dapper;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Features.Profiles.ListProfiles;
using WhoAmI.Application.Results;
using WhoAmI.Infrastructure.Data.Queries.Abstractions;

namespace WhoAmI.Infrastructure.Data.Queries.Features.Profiles.ListProfiles;

internal sealed class ListProfilesQueryHandler :
    IQueryHandler<ListProfilesQuery, IReadOnlyCollection<ListProfilesResponse>>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ListProfilesQueryHandler(IDbConnectionFactory connectionFactory) =>
        _connectionFactory = connectionFactory;

    public async Task<Result<IReadOnlyCollection<ListProfilesResponse>>>
        HandleAsync(
            ListProfilesQuery query,
            CancellationToken cancellationToken = default
        )
    {
        await using var connection = await _connectionFactory
            .CreateOpenConnectionAsync(cancellationToken);

        var command = new CommandDefinition(
            ListProfilesSql.Query,
            cancellationToken: cancellationToken
        );

        var rows  = await connection.QueryAsync<ListProfilesRow>(command);

        var result = rows
            .Select(row => new ListProfilesResponse(
                $"{row.FirstName} {row.LastName}",
                row.Email
            ))
            .ToList();

        return Result<IReadOnlyCollection<ListProfilesResponse>>.Success(
            result.ToList()
        );
    }
}
