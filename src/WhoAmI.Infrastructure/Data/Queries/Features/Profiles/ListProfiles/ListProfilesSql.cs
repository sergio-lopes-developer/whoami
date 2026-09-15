namespace WhoAmI.Infrastructure.Data.Queries.Features.Profiles.ListProfiles;

internal static class ListProfilesSql {
    internal const string Query = """
        SELECT
            first_name AS FirstName,
            last_name AS LastName,
            email AS Email
        FROM Profiles
        ORDER BY first_name, last_name;
    """;
}
