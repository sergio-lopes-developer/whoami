using Microsoft.EntityFrameworkCore;

namespace WhoAmI.Infrastructure.Tests.Persistence.TestInfrastructure;

internal sealed class SqliteContextScope<TContext> :
    IDisposable,
    IAsyncDisposable
    where TContext : DbContext
{
    private readonly SqliteInMemoryDatabase _database;

    public TContext Context { get; }

    public SqliteContextScope(
        Func<DbContextOptions<TContext>,
            TContext> factory
    ) {
        _database = new SqliteInMemoryDatabase();

        var options = new DbContextOptionsBuilder<TContext>()
            .UseSqlite(_database.Connection)
            .Options;

        Context = factory(options);

        Context.Database.EnsureCreated();
    }

    public void Dispose() {
        Context.Dispose();
        _database.Dispose();
    }

    public async ValueTask DisposeAsync() {
        await Context.DisposeAsync();
        await _database.DisposeAsync();
    }
}
