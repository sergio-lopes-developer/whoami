using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Queries;

internal sealed class QueryValidationDecorator<TQuery, TResult> :
    IQueryHandler<TQuery, TResult>,
    IHandlerDecorator<IQueryHandler<TQuery, TResult>>
    where TQuery : IQuery<TResult>
{
    private readonly IEnumerable<IQueryValidator<TQuery>> _validators;

    private readonly IQueryHandler<TQuery, TResult> _inner;

    public IQueryHandler<TQuery, TResult> Inner => _inner;

    public QueryValidationDecorator(
        IQueryHandler<TQuery, TResult> inner,
        IEnumerable<IQueryValidator<TQuery>> validators
    ) {
        _inner = inner;
        _validators = validators;
    }

    public async Task<Result<TResult>> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default
    ) {
        var errors = _validators
            .SelectMany(v => v.Validate(query))
            .Distinct()
            .ToList();

        if (errors.Count != 0) return Result<TResult>.Failure(errors);

        return await _inner.HandleAsync(query, cancellationToken);
    }
}
