using FluentAssertions;
using WhoAmI.Domain.Profiles.Events;
using WhoAmI.Domain.Shared.Base;
using WhoAmI.Domain.Tests.Shared.Base.Dummies;

namespace WhoAmI.Domain.Tests.Shared.Base;

public class AggregateRootTests {
    private static readonly DateTimeOffset _createdAt =
        new(2026, 9, 22, 13, 25, 0, TimeSpan.Zero);

    [Fact]
    public void AggregateRoot_ShouldAddDomainEvent_WhenAddDomainEventIsCalled() {
        // Arrange
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid(), _createdAt);

        var domainEvent = new EmailUpdatedEvent(
            Guid.NewGuid(),
            "email@example.com"
        );

        // Act
        aggregateRoot.AddDomainEventForTest(domainEvent);

        // Assert
        aggregateRoot.DomainEvents
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .BeSameAs(domainEvent);
    }

    [Fact]
    public void AggregateRoot_ShouldRemoveDomainEvent_WhenRemoveDomainEventIsCalled() {
        // Arrange
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid(), _createdAt);

        var domainEvent = new EmailUpdatedEvent(
            Guid.NewGuid(),
            "email@example.com"
        );

        aggregateRoot.AddDomainEventForTest(domainEvent);

        // Act
        aggregateRoot.RemoveDomainEventForTest(domainEvent);

        // Assert
        aggregateRoot.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void AggregateRoot_ShouldNotThrow_WhenRemovingNonExistingDomainEvent() {
        // Arrange
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid(), _createdAt);

        var domainEvent = new EmailUpdatedEvent(
            Guid.NewGuid(),
            "email@example.com"
        );

        // Act
        var act = () => aggregateRoot.RemoveDomainEventForTest(domainEvent);

        // Assert
        act.Should().NotThrow();
        aggregateRoot.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void AggregateRoot_ShouldClearDomainEvents_WhenClearDomainEventsIsCalled() {
        // Arrange
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid(), _createdAt);

        aggregateRoot.AddDomainEventForTest(
            new EmailUpdatedEvent(Guid.NewGuid(), "a@example.com")
        );

        aggregateRoot.AddDomainEventForTest(
            new EmailUpdatedEvent(Guid.NewGuid(), "b@example.com")
        );

        // Act
        aggregateRoot.ClearDomainEvents();

        // Assert
        aggregateRoot.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void AggregateRoot_ShouldStoreMultipleDomainEvents_WhenAddDomainEventIsCalledMultipleTimes() {
        // Arrange
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid(), _createdAt);

        // Act
        aggregateRoot.AddDomainEventForTest(
            new EmailUpdatedEvent(Guid.NewGuid(), "a@example.com")
        );

        aggregateRoot.AddDomainEventForTest(
            new EmailUpdatedEvent(Guid.NewGuid(), "b@example.com")
        );

        // Assert
        aggregateRoot.DomainEvents.Should().HaveCount(2);
    }

    [Fact]
    public void AggregateRoot_ShouldExposeDomainEventsAsReadOnlyCollection() {
        // Arrange
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid(), _createdAt);

        // Assert
        aggregateRoot.DomainEvents
            .Should().BeAssignableTo<IReadOnlyCollection<IDomainEvent>>();

        aggregateRoot.DomainEvents
            .Should().NotBeAssignableTo<List<IDomainEvent>>();
    }

    [Fact]
    public void AggregateRoot_ShouldThrowNotSupportedException_WhenExternalModificationOfDomainEventsIsAttempted() {
        // Arrange
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid(), _createdAt);

        var domainEvents = aggregateRoot.DomainEvents;

        var newEvent = new EmailUpdatedEvent(
            Guid.NewGuid(),
            "email@example.com"
        );

        // Act
        var act = () => ((ICollection<IDomainEvent>)domainEvents).Add(newEvent);

        // Assert
        act.Should().Throw<NotSupportedException>();
    }
}
