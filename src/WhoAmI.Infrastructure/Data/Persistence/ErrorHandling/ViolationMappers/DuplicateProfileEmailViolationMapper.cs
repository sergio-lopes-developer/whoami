using Microsoft.EntityFrameworkCore;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;

namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.ViolationMappers;

internal sealed class DuplicateProfileEmailViolationMapper :
    IPersistenceViolationMapper
{
    public PersistenceViolationCode Code =>
        PersistenceViolationCode.DuplicateProfileEmail;

    public Error Map(DbUpdateException exception) {
        var profile = exception
            .Entries
            .Select(e => e.Entity)
            .OfType<Profile>()
            .Single();

        return ProfileErrors.DuplicateEmail(
            new Email(profile.Email.Address)
        );
    }
}
