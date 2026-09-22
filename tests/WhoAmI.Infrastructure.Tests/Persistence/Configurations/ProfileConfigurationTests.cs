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

    private static IProperty AssertRequiredProperty(
        IProperty? property,
        string columnName
    ) {
        property.Should().NotBeNull();
        property.GetColumnName().Should().Be(columnName);
        property.IsNullable.Should().BeFalse();

        return property;
    }

    [Fact]
    public void Configure_ShouldMapProfileToTable() {
        // Assert
        _entityType.GetTableName().Should().Be("Profiles");
    }

    [Fact]
    public void Configure_ShouldCreateUniqueIdIndex() {
        // Arrange
        var index = _entityType.GetIndexes()
            .Single(i => i.Properties.Single().Name == nameof(Profile.Id));

        // Assert
        index.IsUnique.Should().BeTrue();

        index.GetDatabaseName().Should().Be("UX_Profiles_Guid");
    }

    [Fact]
    public void Configure_ShouldCreateUniqueEmailIndex() {
        // Arrange
        var index = _entityType.GetIndexes()
            .Single(i => i.Properties.Single().Name == nameof(Profile.Email));

        // Assert
        index.IsUnique.Should().BeTrue();

        index.GetDatabaseName().Should().Be("UX_Profiles_Email");
    }

    [Fact]
    public void Configure_ShouldMapIdProperty() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.Id));

        // Assert
        var id = AssertRequiredProperty(property, "id");

        id.ClrType.Should().Be<Guid>();
    }

    [Fact]
    public void Configure_ShouldMapCreatedAtProperty() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.CreatedAt));

        // Assert
        var createdAt = AssertRequiredProperty(property, "created_at");

        createdAt.ClrType.Should().Be<DateTimeOffset>();
    }

    [Fact]
    public void Configure_ShouldMapEmailProperty() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.Email));

        // Assert
        var email = AssertRequiredProperty(property, "email");

        email.ClrType.Should().Be<Email>();
        email.GetMaxLength().Should().Be(Email.MaxLength);
    }

    [Fact]
    public void Configure_ShouldMapLinkedInProperty() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.LinkedIn));

        // Assert
        var linkedIn = AssertRequiredProperty(property, "linkedin_url");

        linkedIn.ClrType.Should().Be<Url>();
    }

    [Fact]
    public void Configure_ShouldMapGitHubProperty() {
        // Arrange
        var property = _entityType.FindProperty(nameof(Profile.GitHub));

        // Assert
        var gitHub = AssertRequiredProperty(property, "github_url");

        gitHub.ClrType.Should().Be<Url>();
    }

    [Fact]
    public void Configure_ShouldConfigureFullNameOwnership() {
        // Arrange
        var navigation = _entityType.FindNavigation(nameof(Profile.FullName));

        // Assert
        navigation.Should().NotBeNull();
        navigation.ForeignKey.IsOwnership.Should().BeTrue();
    }

    [Fact]
    public void Configure_ShouldMapFirstNameProperty() {
        // Arrange
        var firstNameEntity = _model.GetEntityTypes()
            .Single(e => e.ClrType == typeof(FirstName));

        var property = firstNameEntity.FindProperty(nameof(FirstName.Value));

        // Assert
        var firstName = AssertRequiredProperty(property, "first_name");

        firstName.GetMaxLength().Should().Be(FirstName.MaxLength);
    }

    [Fact]
    public void Configure_ShouldMapLastNameProperty() {
        // Arrange
        var lastNameEntity =
            _model.GetEntityTypes().Single(e => e.ClrType == typeof(LastName));

        var property = lastNameEntity.FindProperty(nameof(LastName.Value));

        // Assert
        var lastName = AssertRequiredProperty(property, "last_name");

        lastName.GetMaxLength().Should().Be(LastName.MaxLength);
    }
}
