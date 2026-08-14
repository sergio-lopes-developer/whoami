using Dapper;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.Application.Results;
using WhoAmI.Infrastructure.Data.Queries.Abstractions;

namespace WhoAmI.Infrastructure.Data.Queries.Features.Profiles
    .GetProfileByEmail;

internal sealed class GetProfileByEmailQueryHandler :
    IQueryHandler<GetProfileByEmailQuery, GetProfileByEmailResponse>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetProfileByEmailQueryHandler(
        IDbConnectionFactory connectionFactory
    ) =>
        _connectionFactory = connectionFactory;

    public async Task<Result<GetProfileByEmailResponse>> HandleAsync(
        GetProfileByEmailQuery query,
        CancellationToken cancellationToken = default
    ) {
        await using var connection = await _connectionFactory
            .CreateOpenConnectionAsync(cancellationToken);

        var command = new CommandDefinition(
            GetProfileByEmailSql.Query,
            new { query.Email },
            cancellationToken: cancellationToken
        );

        var row = await connection
            .QuerySingleOrDefaultAsync<GetProfileByEmailRow>(command);

        if (row == null) return ProfileErrors.NotFoundByEmail(query.Email);

        var response = new GetProfileByEmailResponse(
            Guid.Parse(row.Id),
            row.FirstName,
            row.LastName,
            row.Email,
            row.LinkedIn,
            row.GitHub
        );

        return Result<GetProfileByEmailResponse>.Success(response);
    }
}
