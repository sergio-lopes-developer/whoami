using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.CLI.Logging;

namespace WhoAmI.CLI.DependencyInjection;

internal static class ServiceCollectionExtensions {
    public static IServiceCollection AddObservability(
        this IServiceCollection services
    ) {
        services.AddSingleton<IExecutionLogger, ExecutionLogger>();
        return services;
    }
}
