using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.DependencyInjection;
using WhoAmI.Application.Features.Profiles;

namespace WhoAmI.Application.Tests.TestInfrastructure;

internal static class CommandPipelineTestFactory {
    internal static ServiceProvider Create(
        IProfileRepository repo,
        IUnitOfWork uow
    ) {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddApplicationServices();
        services.AddCommandPipeline();

        // Replace real dependencies with test doubles
        services.AddSingleton(repo);
        services.AddSingleton(uow);

        services.AddSingleton(Substitute.For<ISafeExecutionLogger>());

        return services.BuildServiceProvider();
    }
}
