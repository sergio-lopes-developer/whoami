namespace WhoAmI.Application.Results;

public sealed class Result : ResultBase {
    private Result(bool isSuccess, IReadOnlyList<Error> errors)
        : base(isSuccess, errors) { }

    public static Result Success() => new(true, []);

    public static Result Failure(Error error) => new(false, [error]);

    public static Result Failure(IReadOnlyList<Error> errors) =>
        new(false, errors);

    public static implicit operator Result(Error error) => Failure(error);
}

public sealed class Result<T> : ResultBase {
    private T? Value { get; }

    private Result(
        bool isSuccess,
        T? value,
        IReadOnlyList<Error> errors
    ) : base(isSuccess, errors) {
        Value = value;
    }

    public static Result<T> Success(T value) {
        ArgumentNullException.ThrowIfNull(value);

        return new(true, value, []);
    }

    public static Result<T> Failure(Error error) =>
        new(false, default, [error]);

    public static Result<T> Failure(IReadOnlyList<Error> errors) =>
        new(false, default, errors);

    public static implicit operator Result<T>(Error error) => Failure(error);

    public T GetValueOrThrow() {
        if (IsFailure || Value is null) {
            throw new InvalidOperationException(
                "Cannot retrieve the value from a failed result."
            );
        }

        return Value;
    }
}
