using Microsoft.Data.Sqlite;

namespace WhoAmI.Infrastructure.Data.Persistence.Initialization;

internal sealed class SqliteDatabasePreparation : IDatabasePreparation {
    private readonly string _connectionString;

    public SqliteDatabasePreparation(string connectionString) {
        _connectionString = connectionString
            ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public void Prepare() {
        var builder = new SqliteConnectionStringBuilder(
            _connectionString
        );

        if (string.IsNullOrWhiteSpace(builder.DataSource)) {
            throw new InvalidOperationException(
                "The SQLite database path is not configured."
            );
        }

        var directory = Path.GetDirectoryName(
            Path.GetFullPath(builder.DataSource)
        );

        if (!string.IsNullOrWhiteSpace(directory)) {
            Directory.CreateDirectory(directory);
        }
    }
}
