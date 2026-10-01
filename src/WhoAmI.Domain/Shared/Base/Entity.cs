using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Domain.Shared.Guards;

namespace WhoAmI.Domain.Shared.Base;

// ReSharper disable MemberCanBePrivate.Global
public abstract class Entity {
    public Guid Id { get;  private set; } // private set required by EF

    public DateTimeOffset CreatedAt { get; private set; } // private set required by EF

    public DateTimeOffset? UpdatedAt { get; private set; } // private set required by EF

    public DateTimeOffset? DeletedAt { get; private set; } // private set required by EF

    public bool IsDeleted => DeletedAt is not null;

    protected Entity() { } // required by EF

    protected Entity(Guid id, DateTimeOffset createdAt) {
        Guard.AgainstEmptyGuid(id, nameof(id));

        Id = id;
        CreatedAt = createdAt.ToUniversalTime();
    }

    protected void MarkAsDeleted(DateTimeOffset deletedAt) {
        Guard.Against(
            IsDeleted,
            DomainExceptionMessages.EntityAlreadyDeleted
        );

        DeletedAt = deletedAt.ToUniversalTime();
    }

    protected void MarkAsUpdated(DateTimeOffset updatedAt) {
        Guard.Against(
            IsDeleted,
            DomainExceptionMessages.CannotUpdateDeletedEntity
        );

        UpdatedAt = updatedAt.ToUniversalTime();
    }

    protected bool Equals(Entity other) => Id == other.Id;

    public override bool Equals(object? obj) {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Entity)obj);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? a, Entity? b) {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator != (Entity? a, Entity? b) => !(a == b);
}
// ReSharper restore MemberCanBePrivate.Global
