using Microsoft.EntityFrameworkCore;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;

namespace WhoAmI.Infrastructure.Data.Persistence.Context;

public sealed class WhoAmIDbContext : DbContext, IUnitOfWork {
    private readonly IPersistenceErrorMapper _mapper;

    public DbSet<Profile> Profiles => Set<Profile>();

    public WhoAmIDbContext(
        DbContextOptions<WhoAmIDbContext> options,
        IPersistenceErrorMapper mapper
    ) : base(options) =>
        _mapper = mapper;

    public async Task<Result> CommitAsync(
        CancellationToken cancellationToken = default
    ) {
        try {
            await SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException ex) {
            return Result.Failure(_mapper.Map(ex));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WhoAmIDbContext).Assembly
        );
}
