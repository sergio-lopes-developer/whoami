using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.UpdateSocialLinks;

internal sealed class UpdateSocialLinksCommandHandler :
    ICommandHandler<UpdateSocialLinksCommand>
{
    private readonly IProfileRepository _profileRepository;

    public UpdateSocialLinksCommandHandler(
        IProfileRepository profileRepository
    ) =>
        _profileRepository = profileRepository;

    public async Task<Result> HandleAsync(
        UpdateSocialLinksCommand command,
        CancellationToken cancellationToken = default
    ) {
        var profile = await _profileRepository.GetByIdAsync(
            command.Id,
            cancellationToken
        );

        if (profile == null) return ProfileErrors.NotFoundById(command.Id);

        var newLinkedIn = new Url(command.LinkedIn);
        var newGitHub = new Url(command.GitHub);

        profile.UpdateSocialLinks(newLinkedIn, newGitHub);

        return Result.Success();
    }
}
