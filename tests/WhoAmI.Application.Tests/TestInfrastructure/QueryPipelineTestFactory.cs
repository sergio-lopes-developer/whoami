using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.DependencyInjection;
using WhoAmI.Infrastructure.Data.Queries.Abstractions;
using WhoAmI.Infrastructure.Data.Queries.Features.Profiles.GetProfileByEmail;

namespace WhoAmI.Application.Tests.TestInfrastructure;

internal static class QueryPipelineTestFactory {
    internal static ServiceProvider Create(
        IDbConnectionFactory connectionFactory
    ) {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddApplicationServices();
        services.AddSingleton(connectionFactory);

        services.AddSingleton(Substitute.For<ISafeExecutionLogger>());

        services.Scan(scan => scan
            .FromAssemblyOf<GetProfileByEmailQueryHandler>()
            .AddClasses(c => c
                    .AssignableTo(typeof(IQueryHandler<,>)),
                publicOnly: false
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        services.AddQueryPipeline();

        return services.BuildServiceProvider();
    }
}
