namespace WhoAmI.Application.Features.Profiles.ListProfiles;

public sealed record ListProfilesResponse(
    string FirstName,
    string LastName,
    string Email
);
