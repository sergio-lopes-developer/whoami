namespace WhoAmI.Infrastructure.Data.Queries.Profiles.ListProfiles;

internal static class ListProfilesSql {
    internal const string Query = """
        SELECT
            first_name || ' ' || last_name AS FullName,
            email AS Email
        FROM Profiles
        ORDER BY first_name, last_name;
    """;
}
