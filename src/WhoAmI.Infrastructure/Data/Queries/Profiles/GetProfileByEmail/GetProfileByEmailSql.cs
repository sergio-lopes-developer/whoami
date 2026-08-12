namespace WhoAmI.Infrastructure.Data.Queries.Profiles.GetProfileByEmail;

internal static class GetProfileByEmailSql {
    internal const string Query = """
        SELECT
            id AS Id,
            first_name AS FirstName,
            last_name AS LastName,
            email AS Email,
            linkedin_url AS LinkedIn,
            github_url AS GitHub
        FROM Profiles
        WHERE email = @Email
        LIMIT 1;
    """;
}
