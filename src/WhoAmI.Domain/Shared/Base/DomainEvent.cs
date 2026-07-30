namespace WhoAmI.Domain.Shared.Base;

public abstract record DomainEvent : IDomainEvent {
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
