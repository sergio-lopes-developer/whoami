namespace WhoAmI.Application.Features.Profiles.GetProfileByEmail;

public sealed record GetProfileByEmailResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string LinkedIn,
    string GitHub
);
