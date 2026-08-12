namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Exceptions;

internal sealed class UnrecognizedConstraintException : Exception {
    public UnrecognizedConstraintException(string message) : base(message) { }

    public UnrecognizedConstraintException(
        string message,
        Exception innerException
    ) : base(message, innerException) { }
}
