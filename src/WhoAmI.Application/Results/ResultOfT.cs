namespace WhoAmI.Application.Results;

public sealed class Result<T> : ResultBase {
    public T Value { get; }

    private Result(
        bool isSuccess,
        T value,
        IReadOnlyList<Error> errors
    ) : base(isSuccess, errors) {
        if (isSuccess && value is null) {
            throw new ArgumentException("Success result must contain a value.");
        }

        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, []);

    public static Result<T> Failure(Error error) =>
        new(false, default!, [error]);

    public static Result<T> Failure(IReadOnlyList<Error> errors) =>
        new(false, default!, errors);

    public static implicit operator Result<T>(Error error) => Failure(error);
}
