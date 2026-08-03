using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Commands;

internal sealed class CommandValidationDecorator<TCommand, TResult> :
    ICommandHandler<TCommand, TResult>,
    IHandlerDecorator<ICommandHandler<TCommand, TResult>>
    where TCommand : ICommand<TResult>
{
    private readonly IEnumerable<ICommandValidator<TCommand>> _validators;

    private readonly ICommandHandler<TCommand, TResult> _inner;

    public ICommandHandler<TCommand, TResult> Inner => _inner;

    public CommandValidationDecorator(
        ICommandHandler<TCommand, TResult> inner,
        IEnumerable<ICommandValidator<TCommand>> validators
    ) {
        _inner = inner;
        _validators = validators;
    }

    public async Task<Result<TResult>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    ) {
        var errors = _validators
            .SelectMany(v => v.Validate(command))
            .Distinct()
            .ToList();

        if (errors.Count != 0) return Result<TResult>.Failure(errors);

        return await _inner.HandleAsync(command, cancellationToken);
    }
}
