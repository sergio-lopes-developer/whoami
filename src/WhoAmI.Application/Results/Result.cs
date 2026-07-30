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
