using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Abstractions.Serialization;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Decorators.Commands;
using WhoAmI.Application.Decorators.Queries;
using WhoAmI.Application.Dispatching.Commands;
using WhoAmI.Application.Dispatching.Queries;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.Application.Logging;
using WhoAmI.Application.Serialization;
using WhoAmI.Application.Time;

namespace WhoAmI.Application.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services
    ) {
        AddDispatchers(services);
        AddExecutionServices(services);
        AddHandlers(services);
        AddTime(services);
        AddValidations(services);

        return services;
    }

    public static IServiceCollection AddApplicationPipeline(
        this IServiceCollection services
    ) {
        AddCommandPipeline(services);
        AddQueryPipeline(services);

        return services;
    }

    internal static IServiceCollection AddCommandPipeline(
        this IServiceCollection services
    ) {
        services.Decorate(
            typeof(ICommandHandler<>),
            typeof(CommandUnitOfWorkDecorator<>)
        );

        services.Decorate(
            typeof(ICommandHandler<,>),
            typeof(CommandUnitOfWorkDecorator<,>)
        );

        services.Decorate(
            typeof(ICommandHandler<>),
            typeof(CommandValidationDecorator<>)
        );

        services.Decorate(
            typeof(ICommandHandler<,>),
            typeof(CommandValidationDecorator<,>)
        );

        services.Decorate(
            typeof(ICommandHandler<>),
            typeof(CommandExecutionDecorator<>)
        );

        services.Decorate(
            typeof(ICommandHandler<,>),
            typeof(CommandExecutionDecorator<,>)
        );

        return services;
    }

    internal static IServiceCollection AddQueryPipeline(
        this IServiceCollection services
    ) {
        services.Decorate(
            typeof(IQueryHandler<,>),
            typeof(QueryValidationDecorator<,>)
        );

        services.Decorate(
            typeof(IQueryHandler<,>),
            typeof(QueryExecutionDecorator<,>)
        );

        return services;
    }

    private static void AddDispatchers(IServiceCollection services) {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
    }

    private static void AddExecutionServices(IServiceCollection services) {
        services.AddSingleton<ISafeExecutionLogger, SafeExecutionLogger>();
        services.AddSingleton<IObjectSerializer, ObjectSerializer>();
        services.AddSingleton<IExecutionInfoFactory, ExecutionInfoFactory>();
    }

    private static void AddHandlers(IServiceCollection services) {
        services.Scan(scan => scan
            .FromAssemblyOf<CreateProfileCommandHandler>()
            .AddClasses(c => c
                .AssignableTo(typeof(ICommandHandler<>))
                .Where(type => !type.IsGenericTypeDefinition),
                publicOnly: false
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(c => c
                .AssignableTo(typeof(ICommandHandler<,>))
                .Where(type => !type.IsGenericTypeDefinition),
                publicOnly: false
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        services.AddTransient(typeof(CommandHandlerAdapter<>));
        services.AddTransient(typeof(CommandHandlerAdapter<,>));
        services.AddTransient(typeof(QueryHandlerAdapter<,>));
    }

    private static void AddTime(IServiceCollection services) =>
        services.AddSingleton<IClock, Clock>();

    private static void AddValidations(IServiceCollection services) {
        services.Scan(scan => scan
            .FromAssemblyOf<ICommandValidator<object>>()
            .AddClasses(c =>
                c.AssignableTo(typeof(ICommandValidator<>)),
                publicOnly: false
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(c =>
                c.AssignableTo(typeof(IQueryValidator<>)),
                publicOnly: false
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
    }
}
