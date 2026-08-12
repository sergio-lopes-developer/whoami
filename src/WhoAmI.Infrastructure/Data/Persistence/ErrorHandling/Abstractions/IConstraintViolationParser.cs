namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;

internal interface IConstraintViolationParser {
    PersistenceViolationCode Parse(Exception ex);
}
