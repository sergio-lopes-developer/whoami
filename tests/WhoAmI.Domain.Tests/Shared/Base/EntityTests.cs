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
    public void Entity_ShouldThrowDomainException_WhenIdIsEmpty() {
        // Act
        var act = () => new DummyEntity(Guid.Empty, _createdAt);

        // Assert
        act.Should().Throw<DomainException>();
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
