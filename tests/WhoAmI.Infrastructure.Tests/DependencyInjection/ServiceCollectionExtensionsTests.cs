using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Parsers.Sqlite;
using WhoAmI.Infrastructure.DependencyInjection;

namespace WhoAmI.Infrastructure.Tests.DependencyInjection;

public sealed class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddInfrastructure_ShouldRegisterPersistenceServices() {
        // Arrange
        var services = new ServiceCollection();

        var configuration = CreateConfiguration();

        var environment = Substitute.For<IHostEnvironment>();

        // Act
        services.AddInfrastructure(configuration, environment);

        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<WhoAmIDbContext>().Should().NotBeNull();

        provider.GetService<IUnitOfWork>().Should().NotBeNull();

        provider.GetService<IProfileRepository>().Should().NotBeNull();
    }

    [Fact]
    public void AddInfrastructure_ShouldRegisterViolationMappers() {
        // Arrange
        var services = new ServiceCollection();

        var configuration = CreateConfiguration();

        var environment = Substitute.For<IHostEnvironment>();

        // Act
        services.AddInfrastructure(configuration, environment);

        var provider = services.BuildServiceProvider();

        // Assert
        provider
            .GetServices<IPersistenceViolationMapper>()
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public void AddInfrastructure_ShouldRegisterSqliteParser() {
        // Arrange
        var services = new ServiceCollection();

        var configuration = CreateConfiguration();

        var environment = Substitute.For<IHostEnvironment>();

        // Act
        services.AddInfrastructure(configuration, environment);

        var provider = services.BuildServiceProvider();

        // Assert
        provider
            .GetService<IConstraintViolationParser>()
            .Should()
            .BeOfType<SqliteConstraintViolationParser>();
    }

    private static IConfiguration CreateConfiguration() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?> {
                    ["ConnectionStrings:DefaultConnection"] =
                        "Data Source=:memory:"
                }
            )
            .Build();
}
