using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Bootstrap.DependencyInjection;
using WhoAmI.Bootstrap.Initialization;
using WhoAmI.IntegrationTests.TestInfrastructure.Logging;

namespace WhoAmI.IntegrationTests.TestInfrastructure;

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

        // Disable execution logging side effects during tests
        services.AddSingleton<ISafeExecutionLogger, NoOpSafeExecutionLogger>();

        _provider = services.BuildServiceProvider();

        await _provider.InitializeDatabaseAsync();
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
