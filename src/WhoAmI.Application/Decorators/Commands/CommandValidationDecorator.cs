using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Commands;

internal sealed class CommandValidationDecorator<TCommand> :
    ICommandHandler<TCommand>,
    IHandlerDecorator<ICommandHandler<TCommand>>
    where TCommand : ICommand
{
    private readonly IEnumerable<ICommandValidator<TCommand>> _validators;

    private readonly ICommandHandler<TCommand> _inner;

    public ICommandHandler<TCommand> Inner => _inner;

    public CommandValidationDecorator(
        ICommandHandler<TCommand> inner,
        IEnumerable<ICommandValidator<TCommand>> validators
    ) {
        _inner = inner;
        _validators = validators;
    }

    public async Task<Result> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    ) {
        var errors = _validators
            .SelectMany(v => v.Validate(command))
            .Distinct()
            .ToList();

        if (errors.Count != 0) return Result.Failure(errors);

        return await _inner.HandleAsync(command, cancellationToken);
    }
}
