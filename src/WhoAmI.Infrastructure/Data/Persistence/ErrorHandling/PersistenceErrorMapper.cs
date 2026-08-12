using Microsoft.EntityFrameworkCore;
using WhoAmI.Application.Errors;
using WhoAmI.Application.Results;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;

namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling;

internal sealed class PersistenceErrorMapper : IPersistenceErrorMapper {
    private readonly IConstraintViolationParser _parser;

    private readonly IReadOnlyDictionary<
        PersistenceViolationCode,
        IPersistenceViolationMapper
    > _mappers;

    public PersistenceErrorMapper(
        IConstraintViolationParser parser,
        IEnumerable<IPersistenceViolationMapper> mappers
    ) {
        _parser = parser;
        _mappers = mappers.ToDictionary(m => m.Code);
    }

    public Error Map(DbUpdateException exception) {
        var inner = exception.InnerException ?? exception;
        var code = _parser.Parse(inner);

        return !_mappers.TryGetValue(code, out var mapper)
            ? PersistenceErrors.UnmappedConstraint
            : mapper.Map(exception);
    }
}
