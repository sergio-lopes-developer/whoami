using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.UpdateFullName;

internal sealed class UpdateFullNameCommandHandler :
    ICommandHandler<UpdateFullNameCommand>
{
    private readonly IProfileRepository _profileRepository;

    private readonly IClock _clock;

    public UpdateFullNameCommandHandler(
        IProfileRepository profileRepository,
        IClock clock
    ) {
        _profileRepository = profileRepository;
        _clock = clock;
    }

    public async Task<Result> HandleAsync(
        UpdateFullNameCommand command,
        CancellationToken cancellationToken = default
    ) {
        var profile = await _profileRepository.GetByIdAsync(
            command.Id,
            cancellationToken
        );

        if (profile == null) return ProfileErrors.NotFoundById(command.Id);

        var newFullName = new FullName(
            new FirstName(command.FirstName),
            new LastName(command.LastName)
        );

        profile.UpdateFullName(newFullName, _clock.UtcNow);

        return Result.Success();
    }
}
