using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Infrastructure.Data.Persistence.Context;

namespace WhoAmI.Bootstrap.Initialization;

public static class DatabaseInitializationExtensions {
    public static async Task InitializeDatabaseAsync(
        this IServiceProvider services
    ) {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<WhoAmIDbContext>();

        await db.Database.MigrateAsync();
    }
}
