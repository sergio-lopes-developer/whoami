using FluentAssertions;
using WhoAmI.Domain.Profiles.Events;
using WhoAmI.Domain.Shared.Base;
using WhoAmI.Domain.Tests.Shared.Base.Dummies;

namespace WhoAmI.Domain.Tests.Shared.Base;

public class AggregateRootTests {
    [Fact]
    public void AggregateRoot_ShouldAddDomainEvent() {
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid());
        var domainEvent = new EmailUpdatedEvent(
            Guid.NewGuid(),
            "email@mail.com"
        );

        aggregateRoot.AddDomainEventForTest(domainEvent);

        aggregateRoot.DomainEvents
            .Should().ContainSingle(e => (EmailUpdatedEvent)e == domainEvent);
    }

    [Fact]
    public void AggregateRoot_ShouldRemoveDomainEvent() {
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid());
        var domainEvent = new EmailUpdatedEvent(
            Guid.NewGuid(),
            "email@mail.com"
        );
        aggregateRoot.AddDomainEventForTest(domainEvent);

        aggregateRoot.RemoveDomainEventForTest(domainEvent);

        aggregateRoot.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void AggregateRoot_ShouldClearDomainEvents() {
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid());
        aggregateRoot.AddDomainEventForTest(
            new EmailUpdatedEvent(Guid.NewGuid(), "a@mail.com")
        );
        aggregateRoot.AddDomainEventForTest(
            new EmailUpdatedEvent(Guid.NewGuid(), "b@mail.com")
        );

        aggregateRoot.ClearDomainEvents();

        aggregateRoot.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void AggregateRoot_ShouldStoreMultipleDomainEvents() {
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid());

        aggregateRoot.AddDomainEventForTest(
            new EmailUpdatedEvent(Guid.NewGuid(), "a@mail.com")
        );
        aggregateRoot.AddDomainEventForTest(
            new EmailUpdatedEvent(Guid.NewGuid(), "b@mail.com")
        );

        aggregateRoot.DomainEvents.Should().HaveCount(2);
    }

    [Fact]
    public void AggregateRoot_ShouldExposeReadOnlyCollection() {
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid());

        aggregateRoot.DomainEvents
            .Should().BeAssignableTo<IReadOnlyCollection<IDomainEvent>>();
        aggregateRoot.DomainEvents
            .Should().NotBeAssignableTo<List<IDomainEvent>>();
    }

    [Fact]
    public void AggregateRoot_ShouldNotAllowExternalModificationOfDomainEvents() {
        var aggregateRoot = new DummyAggregateRoot(Guid.NewGuid());
        var domainEvents = aggregateRoot.DomainEvents;
        var newEvent = new EmailUpdatedEvent(Guid.NewGuid(), "email@mail.com");

        var act = () => ((ICollection<IDomainEvent>)domainEvents).Add(newEvent);

        act.Should().Throw<NotSupportedException>();
    }
}
