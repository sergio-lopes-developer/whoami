using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.CreateProfile;

internal sealed class CreateProfileCommandHandler :
    ICommandHandler<CreateProfileCommand, CreateProfileResponse>
{
    private readonly IProfileRepository _profileRepository;

    public CreateProfileCommandHandler(IProfileRepository profileRepository) =>
        _profileRepository = profileRepository;

    public async Task<Result<CreateProfileResponse>> HandleAsync(
        CreateProfileCommand command,
        CancellationToken cancellationToken = default
    ) {
        var email = new Email(command.Email);
        var linkedIn = new Url(command.LinkedIn);
        var gitHub = new Url(command.GitHub);
        var fullName = new FullName(
            new FirstName(command.FirstName),
            new LastName(command.LastName)
        );

        var profile = Profile.Create(fullName, email, linkedIn, gitHub);

        _profileRepository.Add(profile);

        var result = new CreateProfileResponse(
            profile.Id,
            profile.FullName.FirstName.Value,
            profile.FullName.LastName.Value,
            profile.Email.Address,
            profile.LinkedIn.Value,
            profile.GitHub.Value
        );

        return Result<CreateProfileResponse>.Success(result);
    }
}
