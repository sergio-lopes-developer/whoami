using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Validation;

internal interface ICommandValidator<in TCommand> {
    IReadOnlyCollection<Error> Validate(TCommand command);
}
