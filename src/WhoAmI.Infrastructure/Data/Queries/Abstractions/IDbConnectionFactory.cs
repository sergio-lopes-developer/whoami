using System.Data.Common;

namespace WhoAmI.Infrastructure.Data.Queries.Abstractions;

internal interface IDbConnectionFactory {
    Task<DbConnection> CreateOpenConnectionAsync(
        CancellationToken cancellationToken = default
    );
}
