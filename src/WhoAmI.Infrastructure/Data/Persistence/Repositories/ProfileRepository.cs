using Microsoft.EntityFrameworkCore;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Domain.Profiles;
using WhoAmI.Infrastructure.Data.Persistence.Context;

namespace WhoAmI.Infrastructure.Data.Persistence.Repositories;

internal sealed class ProfileRepository : IProfileRepository {
    private readonly WhoAmIDbContext _context;

    public ProfileRepository(WhoAmIDbContext context) => _context = context;

    public Task<Profile?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) =>
        _context
            .Profiles
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public void Add(Profile profile) => _context.Profiles.Add(profile);
}
