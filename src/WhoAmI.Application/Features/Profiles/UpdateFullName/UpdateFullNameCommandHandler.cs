using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.UpdateFullName;

internal sealed class UpdateFullNameCommandHandler :
    ICommandHandler<UpdateFullNameCommand>
{
    private readonly IProfileRepository _profileRepository;

    public UpdateFullNameCommandHandler(IProfileRepository profileRepository) =>
        _profileRepository = profileRepository;

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

        profile.UpdateFullName(newFullName);

        return Result.Success();
    }
}
