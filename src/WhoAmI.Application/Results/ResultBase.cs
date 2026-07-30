namespace WhoAmI.Application.Results;

public abstract class ResultBase {
    public bool IsSuccess { get; }

    public IReadOnlyList<Error> Errors { get; }

    protected ResultBase(bool isSuccess, IReadOnlyList<Error> errors) {
        if (!isSuccess && errors.Count == 0) {
            throw new ArgumentException("Failure result must contain errors.");
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsFailure => !IsSuccess;

    public Error? FirstError => Errors.FirstOrDefault();
}
