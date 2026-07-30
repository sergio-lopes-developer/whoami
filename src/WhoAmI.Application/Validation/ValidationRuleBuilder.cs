using WhoAmI.Application.Errors;
using WhoAmI.Application.Results;
using WhoAmI.Domain.Shared.Constraints;

namespace WhoAmI.Application.Validation;

internal sealed class ValidationRuleBuilder<T> {
    private List<Error>? _errors;

    private readonly T _value;

    private readonly string _field;

    internal ValidationRuleBuilder(T value, string field) {
        _value = value;
        _field = field;
    }

    internal ValidationRuleBuilder<T> IsRequired() {
        switch (_value) {
            case null:

            case string text when string.IsNullOrWhiteSpace(text):

            case Guid guid when guid == Guid.Empty:
                AddError(ValidationErrors.Required(_field));
                break;
        }

        return this;
    }

    internal ValidationRuleBuilder<T> Satisfies(
        Func<string?, bool> isSatisfiedBy
    ) {
        if (_value is not string text || string.IsNullOrWhiteSpace(text)) {
            return this;
        }

        if (!isSatisfiedBy(text)) {
            AddError(ValidationErrors.InvalidFormat(_field));
        }

        return this;
    }

    internal ValidationRuleBuilder<T> HasValidLength(
        TextLengthConstraint constraint
    ) {
        if (_value is not string text || string.IsNullOrWhiteSpace(text)) {
            return this;
        }

        if (!constraint.IsSatisfiedBy(text)) {
            AddError(
                ValidationErrors.InvalidLength(
                    _field,
                    constraint.MinLength,
                    constraint.MaxLength
                )
            );
        }

        return this;
    }

    private void AddError(Error error) => (_errors ??= []).Add(error);

    internal void AddTo(List<Error> errors) {
        if (_errors is null) {
            return;
        }

        errors.AddRange(_errors);
    }

    internal IReadOnlyCollection<Error> ToErrors() =>
        _errors?.AsReadOnly() ?? [];
}
