using WhoAmI.Domain.Profiles;

namespace WhoAmI.Application.Features.Profiles;

public interface IProfileRepository {
    Task<Profile?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    void Add(Profile profile);
}
