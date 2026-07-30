using WhoAmI.Domain.Shared.Base;

namespace WhoAmI.Domain.Tests.Shared.Base.Dummies;

internal sealed class DummyAggregateRoot(Guid id) : AggregateRoot(id) {
    public void AddDomainEventForTest(IDomainEvent domainEvent) =>
        AddDomainEvent(domainEvent);

    public void RemoveDomainEventForTest(IDomainEvent domainEvent) =>
        RemoveDomainEvent(domainEvent);
}
