namespace WhoAmI.Application.Features.Profiles.CreateProfile;

public sealed record CreateProfileResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string LinkedIn,
    string GitHub
);
