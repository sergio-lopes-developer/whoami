using Microsoft.EntityFrameworkCore;
using WhoAmI.Application.Results;

namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;

public interface IPersistenceErrorMapper {
    Error Map(DbUpdateException exception);
}
