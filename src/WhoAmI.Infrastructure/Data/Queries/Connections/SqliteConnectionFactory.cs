using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using WhoAmI.Infrastructure.Data.Queries.Abstractions;

namespace WhoAmI.Infrastructure.Data.Queries.Connections;

internal sealed class SqliteConnectionFactory : IDbConnectionFactory {
    private readonly string _connectionString;

    public SqliteConnectionFactory(IConfiguration configuration) {
        _connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "The connection string 'DefaultConnection' was not configured."
            );
    }

    public async Task<DbConnection> CreateOpenConnectionAsync(
        CancellationToken cancellationToken = default
    ) {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        return connection;
    }
}
