using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WhoAmI.Application.DependencyInjection;
using WhoAmI.Infrastructure.DependencyInjection;

namespace WhoAmI.Bootstrap.DependencyInjection;

public static class BootstrapExtensions {
    public static IServiceCollection AddBootstrap(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        Action<IServiceProvider, DbContextOptionsBuilder>? configureDb = null
    ) {
        services
            .AddApplicationServices()
            .AddInfrastructure(configuration, environment, configureDb)
            .AddApplicationPipeline();

        return services;
    }
}
