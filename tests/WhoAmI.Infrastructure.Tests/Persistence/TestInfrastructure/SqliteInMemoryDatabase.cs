using Microsoft.Data.Sqlite;

namespace WhoAmI.Infrastructure.Tests.Persistence.TestInfrastructure;

internal sealed class SqliteInMemoryDatabase : IDisposable, IAsyncDisposable {
    private readonly SqliteConnection _connection;

    public SqliteConnection Connection => _connection;

    public SqliteInMemoryDatabase() {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
    }

    public void Dispose() => _connection.Dispose();

    public ValueTask DisposeAsync() => _connection.DisposeAsync();
}
