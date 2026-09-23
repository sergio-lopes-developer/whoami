namespace WhoAmI.Domain.Shared.Base;

public abstract class AggregateRoot : Entity {
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents =>
        _domainEvents.AsReadOnly();

    protected AggregateRoot() { } // for EF

    protected AggregateRoot(
        Guid id,
        DateTimeOffset createdAt
    ) : base(
        id,
        createdAt
    ) { }

    protected void AddDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    protected void RemoveDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
