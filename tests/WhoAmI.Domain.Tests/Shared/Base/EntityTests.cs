using FluentAssertions;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Domain.Tests.Shared.Base.Dummies;

namespace WhoAmI.Domain.Tests.Shared.Base;

public class EntityTests {
    private static readonly DateTimeOffset _createdAt =
        new(2026, 9, 22, 13, 25, 0, TimeSpan.Zero);

    [Fact]
    public void Entity_ShouldBeCreated_WhenIdIsValid() {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var entity = new DummyEntity(id, _createdAt);

        // Assert
        entity.Id.Should().Be(id);
        entity.CreatedAt.Should().Be(_createdAt);
        entity.UpdatedAt.Should().BeNull();
        entity.DeletedAt.Should().BeNull();
        entity.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Entity_ShouldStoreCreatedAtAsUtc_WhenLocalTimeIsProvided() {
        // Arrange
        var createdAt = new DateTimeOffset(
            2026, 9, 22,
            11, 30, 0,
            TimeSpan.FromHours(-3)
        );

        // Act
        var entity = new DummyEntity(Guid.NewGuid(), createdAt);

        // Assert
        entity.CreatedAt.Should().Be(createdAt.ToUniversalTime());
    }

    [Fact]
    public void Entity_ShouldStoreDeletedAtAsUtc_WhenMarkedAsDeleted() {
        // Arrange
        var entity = new DummyEntity(
            Guid.NewGuid(),
            _createdAt
        );

        var deletedAt = new DateTimeOffset(
            2026, 9, 23,
            10, 15, 0,
            TimeSpan.FromHours(-3)
        );

        // Act
        entity.InvokeMarkAsDeleted(deletedAt);

        // Assert
        entity.IsDeleted.Should().BeTrue();
        entity.DeletedAt.Should().Be(deletedAt.ToUniversalTime());
    }

    [Fact]
    public void Entity_ShouldStoreUpdatedAtAsUtc_WhenMarkedAsUpdated() {
        // Arrange
        var entity = new DummyEntity(
            Guid.NewGuid(),
            _createdAt
        );

        var updatedAt = new DateTimeOffset(
            2026, 9, 23,
            10, 15, 0,
            TimeSpan.FromHours(-3)
        );

        // Act
        entity.InvokeMarkAsUpdated(updatedAt);

        // Assert
        entity.UpdatedAt.Should().Be(updatedAt.ToUniversalTime());
    }

    [Fact]
    public void Entity_ShouldThrowDomainException_WhenIdIsEmpty() {
        // Act
        var act = () => new DummyEntity(Guid.Empty, _createdAt);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Entity_ShouldThrowDomainException_WhenDeletingDeletedEntity() {
        // Arrange
        var entity = new DummyEntity(Guid.NewGuid(), _createdAt);

        var firstDeletion = new DateTimeOffset(
            2026, 9, 29,
            14, 30, 0,
            TimeSpan.FromHours(-3)
        );

        var secondDeletion = new DateTimeOffset(
            2026, 9, 29,
            15, 45, 0,
            TimeSpan.FromHours(-3)
        );

        entity.InvokeMarkAsDeleted(firstDeletion);

        // Act
        var act = () => entity.InvokeMarkAsDeleted(secondDeletion);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage(DomainExceptionMessages.EntityAlreadyDeleted);
    }

    [Fact]
    public void Entity_ShouldThrowDomainException_WhenUpdatingDeletedEntity() {
        // Arrange
        var entity = new DummyEntity(Guid.NewGuid(), _createdAt);

        var deletedAt = new DateTimeOffset(
            2026, 9, 23,
            10, 15, 0,
            TimeSpan.FromHours(-3)
        );

        entity.InvokeMarkAsDeleted(deletedAt);

        var updatedAt = new DateTimeOffset(
            2026, 9, 29,
            12, 45, 0,
            TimeSpan.FromHours(-3)
        );

        // Act
        var act = () => entity.InvokeMarkAsUpdated(updatedAt);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage(DomainExceptionMessages.CannotUpdateDeletedEntity);
    }

    [Fact]
    public void Entity_ShouldBeEqual_WhenIdsAreIdentical() {
        // Arrange
        var id = Guid.NewGuid();

        var entity = new DummyEntity(
            id,
            new DateTimeOffset(2026, 9, 22, 14, 30, 0, TimeSpan.Zero)
        );

        var sameEntity = new DummyEntity(
            id,
            new DateTimeOffset(2030, 1, 1, 8, 0, 0, TimeSpan.Zero)
        );

        // Assert
        entity.Should().Be(sameEntity);
        entity.GetHashCode().Should().Be(sameEntity.GetHashCode());
        (entity == sameEntity).Should().BeTrue();
        (entity != sameEntity).Should().BeFalse();
    }

    [Fact]
    public void Entity_ShouldNotBeEqual_WhenIdsAreDifferent() {
        // Arrange
        var entity = new DummyEntity(
            Guid.NewGuid(),
            _createdAt
        );

        var otherEntity = new DummyEntity(
            Guid.NewGuid(),
            _createdAt
        );

        // Assert
        entity.Should().NotBe(otherEntity);
        (entity != otherEntity).Should().BeTrue();
        (entity == otherEntity).Should().BeFalse();
    }
}
