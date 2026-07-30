using WhoAmI.Domain.Shared.Base;

namespace WhoAmI.Domain.Profiles.Events;

public sealed record EmailUpdatedEvent(
    Guid ProfileId,
    string NewEmail
) : DomainEvent;
