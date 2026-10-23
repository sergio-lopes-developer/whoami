using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.Infrastructure.Data.Persistence.Initialization;

namespace WhoAmI.Bootstrap.Initialization;

public static class DatabaseInitializationExtensions {
    public static async Task InitializeDatabaseAsync(
        this IServiceProvider services
    ) {
        using var scope = services.CreateScope();

        PrepareDatabase(scope.ServiceProvider);

        await MigrateDatabaseAsync(scope.ServiceProvider);
    }

    private static void PrepareDatabase(IServiceProvider services) {
        var preparation = services.GetRequiredService<IDatabasePreparation>();
        preparation.Prepare();
    }

    private static async Task MigrateDatabaseAsync(IServiceProvider services) {
        var db = services.GetRequiredService<WhoAmIDbContext>();
        await db.Database.MigrateAsync();
    }
}
