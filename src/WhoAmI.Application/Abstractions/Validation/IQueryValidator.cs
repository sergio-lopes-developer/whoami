using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Validation;

internal interface IQueryValidator<in TQuery> {
    IReadOnlyCollection<Error> Validate(TQuery query);
}
