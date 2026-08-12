namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling;

internal sealed record PersistenceViolationCode(string Value) {
    public static readonly PersistenceViolationCode DuplicateProfileEmail =
        new(nameof(DuplicateProfileEmail));

    public static readonly PersistenceViolationCode DuplicateProfileGuid =
        new(nameof(DuplicateProfileGuid));

    public override string ToString() => Value;
}
