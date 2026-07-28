namespace WhoAmI.Domain.Shared.Base;

// ReSharper disable MemberCanBePrivate.Global
public abstract class ValueObject {
    protected abstract IEnumerable<object?> GetEqualityComponents();

    protected bool Equals(ValueObject other) =>
        GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override bool Equals(object? obj) {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((ValueObject)obj);
    }

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(0, HashCode.Combine);

    public static bool operator ==(ValueObject? a, ValueObject? b) {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);
}
// ReSharper restore MemberCanBePrivate.Global
