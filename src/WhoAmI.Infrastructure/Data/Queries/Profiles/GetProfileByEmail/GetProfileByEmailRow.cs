namespace WhoAmI.Infrastructure.Data.Queries.Profiles.GetProfileByEmail;

internal sealed record GetProfileByEmailRow(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string LinkedIn,
    string GitHub
);
