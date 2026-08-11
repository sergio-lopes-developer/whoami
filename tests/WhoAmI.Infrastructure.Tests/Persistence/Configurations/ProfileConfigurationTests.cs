using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NSubstitute;
using WhoAmI.Domain.Profiles;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;
using WhoAmI.Infrastructure.Tests.Persistence.TestInfrastructure;

namespace WhoAmI.Infrastructure.Tests.Persistence.Configurations;

public sealed class ProfileConfigurationTests : IAsyncDisposable {
    private readonly SqliteContextScope<WhoAmIDbContext> _scope;

    private readonly IModel _model;

    private readonly IEntityType _entityType;

    public ProfileConfigurationTests() {
        var mapper = Substitute.For<IPersistenceErrorMapper>();

        _scope = new SqliteContextScope<WhoAmIDbContext>(
            options => new WhoAmIDbContext(options, mapper)
        );

        _model = _scope.Context.Model;

        _entityType = _model.FindEntityType(typeof(Profile))
            ?? throw new InvalidOperationException(
                "Profile entity type was not found."
            );
    }

    public ValueTask DisposeAsync() => _scope.DisposeAsync();

    private static void AssertRequiredProperty(
        IProperty? property,
        string columnName
    ) {
        property.Should().NotBeNull();
        property!.GetColumnName().Should().Be(columnName);
        property.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapProfileToProfilesTable() {
        // Assert
        _entityType.GetTableName().Should().Be("Profiles");
    }

    [Fact]
    public void Configure_ShouldMapIdPropertyCorrectly() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.Id));

        // Assert
        AssertRequiredProperty(property, "id");
    }

    [Fact]
    public void Configure_ShouldCreateUniqueIndexForId() {
        // Arrange
        var index = _entityType.GetIndexes()
            .Single(i => i.Properties.Single().Name == nameof(Profile.Id));

        // Assert
        index.IsUnique.Should().BeTrue();

        index.GetDatabaseName().Should().Be("UX_Profiles_Guid");
    }

    [Fact]
    public void Configure_ShouldCreateUniqueIndexForEmail() {
        // Arrange
        var index = _entityType.GetIndexes()
            .Single(i => i.Properties.Single().Name == nameof(Profile.Email));

        // Assert
        index.IsUnique.Should().BeTrue();

        index.GetDatabaseName().Should().Be("UX_Profiles_Email");
    }

    [Fact]
    public void Configure_ShouldMapEmailPropertyCorrectly() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.Email));

        // Assert
        AssertRequiredProperty(property, "email");

        property!.GetMaxLength().Should().Be(Email.MaxLength);
    }

    [Fact]
    public void Configure_ShouldMapLinkedInPropertyCorrectly() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.LinkedIn));

        // Assert
        AssertRequiredProperty(property, "linkedin_url");
    }

    [Fact]
    public void Configure_ShouldMapGitHubPropertyCorrectly() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.GitHub));

        // Assert
        AssertRequiredProperty(property, "github_url");
    }

    [Fact]
    public void Configure_ShouldConfigureFullNameAsOwnedEntity() {
        // Arrange
        var navigation = _entityType.FindNavigation(nameof(Profile.FullName));

        // Assert
        navigation.Should().NotBeNull();

        navigation!.ForeignKey.IsOwnership.Should().BeTrue();
    }

    [Fact]
    public void Configure_ShouldMapFirstNamePropertyCorrectly() {
        // Arrange
        var firstNameEntity = _model.GetEntityTypes()
            .Single(e => e.ClrType == typeof(FirstName));

        var property = firstNameEntity.FindProperty(nameof(FirstName.Value));

        // Assert
        AssertRequiredProperty(property, "first_name");

        property!.GetMaxLength().Should().Be(FirstName.MaxLength);
    }

    [Fact]
    public void Configure_ShouldMapLastNamePropertyCorrectly() {
        // Arrange
        var lastNameEntity =
            _model.GetEntityTypes().Single(e => e.ClrType == typeof(LastName));

        var property = lastNameEntity.FindProperty(nameof(LastName.Value));

        // Assert
        AssertRequiredProperty(property, "last_name");

        property!.GetMaxLength().Should().Be(LastName.MaxLength);
    }
}
