using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Bootstrap.DependencyInjection;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.IntegrationTests.Infrastructure.Logging;

namespace WhoAmI.IntegrationTests.Infrastructure;

public sealed class TestApplicationFactory : IAsyncLifetime {
    const string ConnectionString =
        "Data Source=WhoAmI_Test;" +
        "Mode=Memory;" +
        "Cache=Shared";

    private ServiceProvider _provider = default!;

    private SqliteConnection _connection = default!;

    public async ValueTask InitializeAsync() {
        var services = new ServiceCollection();

        // Keep DB alive for entire test suite
        _connection = new SqliteConnection(ConnectionString);
        await _connection.OpenAsync();

        services.AddSingleton(_connection);

        services.AddLogging(x => x.SetMinimumLevel(LogLevel.Warning));

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?> {
                    ["ConnectionStrings:DefaultConnection"] = ConnectionString
                }
            )
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        var environment = new TestHostEnvironment();

        services.AddBootstrap(
            configuration,
            environment,
            (_, options) => { options.UseSqlite(_connection); }
        );

        // override production observer for tests
        services.AddSingleton<ISafeExecutionLogger, NoOpSafeExecutionLogger>();

        _provider = services.BuildServiceProvider();

        // Create schema once
        using var scope = _provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WhoAmIDbContext>();

        await db.Database.MigrateAsync();
    }

    public async ValueTask DisposeAsync() {
        await _connection.DisposeAsync();
        await _provider.DisposeAsync();
    }

    public async Task<IntegrationTestScope> CreateScopeAsync() =>
        await IntegrationTestScope.CreateAsync(
            _provider,
            useTransaction: false
        );

    public async Task<IntegrationTestScope> CreateTransactionalScopeAsync() =>
        await IntegrationTestScope.CreateAsync(
            _provider,
            useTransaction: true
        );
}
