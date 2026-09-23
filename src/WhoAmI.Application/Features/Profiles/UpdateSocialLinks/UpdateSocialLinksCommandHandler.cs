using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.UpdateSocialLinks;

internal sealed class UpdateSocialLinksCommandHandler :
    ICommandHandler<UpdateSocialLinksCommand>
{
    private readonly IProfileRepository _profileRepository;

    private readonly IClock _clock;

    public UpdateSocialLinksCommandHandler(
        IProfileRepository profileRepository,
        IClock clock
    ) {
        _profileRepository = profileRepository;
        _clock = clock;
    }

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

        profile.UpdateSocialLinks(newLinkedIn, newGitHub, _clock.UtcNow);

        return Result.Success();
    }
}
