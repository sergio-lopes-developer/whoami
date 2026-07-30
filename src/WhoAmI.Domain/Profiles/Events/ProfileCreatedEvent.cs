using WhoAmI.Domain.Shared.Base;

namespace WhoAmI.Domain.Profiles.Events;

public sealed record ProfileCreatedEvent(Guid ProfileId) : DomainEvent;
