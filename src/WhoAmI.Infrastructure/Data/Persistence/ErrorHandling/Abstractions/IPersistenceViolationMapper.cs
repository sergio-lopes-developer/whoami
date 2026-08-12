using Microsoft.EntityFrameworkCore;
using WhoAmI.Application.Results;

namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;

internal interface IPersistenceViolationMapper {
    PersistenceViolationCode Code { get; }

    Error Map(DbUpdateException exception);
}
