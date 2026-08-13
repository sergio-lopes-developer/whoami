using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Parsers.Sqlite;
using WhoAmI.Infrastructure.Data.Persistence.Repositories;
using WhoAmI.Infrastructure.Data.Queries.Abstractions;
using WhoAmI.Infrastructure.Data.Queries.Connections;
using WhoAmI.Infrastructure.Data.Queries.Profiles.GetProfileByEmail;

namespace WhoAmI.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions {
    private const string DefaultConnection = "DefaultConnection";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        Action<IServiceProvider, DbContextOptionsBuilder>? configureDb = null
    ) {
        AddPersistence(services, configuration, environment, configureDb);
        AddQueryInfrastructure(services);
        AddQueries(services);
        AddErrorHandling(services);

        return services;
    }

    private static void AddErrorHandling(IServiceCollection services) {
        services.Scan(scan => scan
            .FromAssemblyOf<IPersistenceViolationMapper>()
            .AddClasses(classes =>
                classes.AssignableTo<IPersistenceViolationMapper>(),
                publicOnly: false
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<
            IConstraintViolationParser,
            SqliteConstraintViolationParser
        >();

        services.AddScoped<IPersistenceErrorMapper, PersistenceErrorMapper>();
    }

    private static void AddPersistence(
        IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        Action<IServiceProvider, DbContextOptionsBuilder>? configureDb
    ) {
        services.AddDbContext<WhoAmIDbContext>((sp, options) => {
            ConfigureDbContext(
                sp,
                options,
                configuration,
                environment,
                configureDb
            );
        });

        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<WhoAmIDbContext>()
        );

        services.AddScoped<IProfileRepository, ProfileRepository>();
    }

    private static void AddQueries(IServiceCollection services) {
        services.Scan(scan => scan
            .FromAssemblyOf<GetProfileByEmailQueryHandler>()
            .AddClasses(c => c
                .AssignableTo(typeof(IQueryHandler<,>))
                .Where(type => !type.IsGenericTypeDefinition),
                publicOnly: false
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
    }

    private static void AddQueryInfrastructure(IServiceCollection services) =>
        services.AddScoped<IDbConnectionFactory, SqliteConnectionFactory>();

    private static void ConfigureDbContext(
        IServiceProvider serviceProvider,
        DbContextOptionsBuilder options,
        IConfiguration configuration,
        IHostEnvironment environment,
        Action<IServiceProvider, DbContextOptionsBuilder>? configureDb
    ) {
        if (configureDb is not null) {
            configureDb(serviceProvider, options);
        }
        else {
            options.UseSqlite(
                configuration.GetConnectionString(DefaultConnection)
            );
        }

        if (environment.IsDevelopment()) {
            options
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }
}
