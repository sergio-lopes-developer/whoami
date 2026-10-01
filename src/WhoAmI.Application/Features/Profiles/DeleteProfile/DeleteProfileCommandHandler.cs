using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Features.Profiles.DeleteProfile;

internal sealed class DeleteProfileCommandHandler :
    ICommandHandler<DeleteProfileCommand>
{
    private readonly IProfileRepository _profileRepository;

    private readonly IClock _clock;

    public DeleteProfileCommandHandler(
        IProfileRepository profileRepository,
        IClock clock
    ) {
        _profileRepository = profileRepository;
        _clock = clock;
    }

    public async Task<Result> HandleAsync(
        DeleteProfileCommand command,
        CancellationToken cancellationToken
    ) {
        var profile = await _profileRepository.GetByIdAsync(
            command.Id,
            cancellationToken
        );

        if (profile == null) return ProfileErrors.NotFoundById(command.Id);

        if (profile.IsDeleted) return ProfileErrors.AlreadyDeleted(command.Id);

        profile.Delete(_clock.UtcNow);

        return Result.Success();
    }

}
