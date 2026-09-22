using WhoAmI.Domain.Shared.Base;

namespace WhoAmI.Domain.Tests.Shared.Base.Dummies;

internal sealed class DummyEntity(
    Guid id,
    DateTimeOffset createdAt
) : Entity(
    id,
    createdAt
);
