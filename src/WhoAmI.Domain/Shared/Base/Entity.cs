using WhoAmI.Domain.Shared.Guards;

namespace WhoAmI.Domain.Shared.Base;

// ReSharper disable MemberCanBePrivate.Global
public abstract class Entity {
    public Guid Id { get;  private set; } // private set required by EF

    protected Entity() { } // required by EF

    protected Entity(Guid id) {
        Guard.AgainstEmptyGuid(id, nameof(id));
        Id = id;
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
