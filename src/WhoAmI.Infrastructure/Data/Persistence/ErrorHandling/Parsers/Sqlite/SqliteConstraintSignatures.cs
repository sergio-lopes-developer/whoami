namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Parsers.Sqlite;

internal static class SqliteConstraintSignatures {
    internal static readonly IReadOnlyDictionary<
        string,
        PersistenceViolationCode
    > Registry =
        new Dictionary<string, PersistenceViolationCode>(
            StringComparer.OrdinalIgnoreCase
        ) {
            ["Profiles.email"] = PersistenceViolationCode.DuplicateProfileEmail,
            ["Profiles.id"] = PersistenceViolationCode.DuplicateProfileGuid
        };
}
