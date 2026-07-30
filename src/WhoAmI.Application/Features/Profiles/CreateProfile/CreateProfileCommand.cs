using WhoAmI.Application.Abstractions.Commands;

namespace WhoAmI.Application.Features.Profiles.CreateProfile;

public sealed record CreateProfileCommand(
    string FirstName,
    string LastName,
    string Email,
    string LinkedIn,
    string GitHub
) : ICommand<CreateProfileResponse>;
