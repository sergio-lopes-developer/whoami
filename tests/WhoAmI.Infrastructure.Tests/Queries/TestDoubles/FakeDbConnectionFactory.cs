using System.Data.Common;
using Microsoft.Data.Sqlite;
using WhoAmI.Infrastructure.Data.Queries.Abstractions;

namespace WhoAmI.Infrastructure.Tests.Queries.TestDoubles;

public sealed class FakeDbConnectionFactory : IDbConnectionFactory {
    private readonly SqliteConnection _connection;

    public FakeDbConnectionFactory(SqliteConnection connection) =>
        _connection = connection;

    public Task<DbConnection> CreateOpenConnectionAsync(
        CancellationToken cancellationToken = default
    ) =>
        Task.FromResult<DbConnection>(_connection);
}
