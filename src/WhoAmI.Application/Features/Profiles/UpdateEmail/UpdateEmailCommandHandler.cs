using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.UpdateEmail;

internal sealed class UpdateEmailCommandHandler :
    ICommandHandler<UpdateEmailCommand>
{
    private readonly IProfileRepository _profileRepository;

    private readonly IClock _clock;

    public UpdateEmailCommandHandler(
        IProfileRepository profileRepository,
        IClock clock
    ) {
        _profileRepository = profileRepository;
        _clock = clock;
    }

    public async Task<Result> HandleAsync(
        UpdateEmailCommand command,
        CancellationToken cancellationToken = default
    ) {
        var profile = await _profileRepository.GetByIdAsync(
            command.Id,
            cancellationToken
        );

        if (profile == null) return ProfileErrors.NotFoundById(command.Id);

        var newEmail = new Email(command.Email);

        profile.UpdateEmail(newEmail, _clock.UtcNow);

        return Result.Success();
    }
}
