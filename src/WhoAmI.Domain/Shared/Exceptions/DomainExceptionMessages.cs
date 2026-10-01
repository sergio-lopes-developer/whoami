namespace WhoAmI.Domain.Shared.Exceptions;

public static class DomainExceptionMessages {
    public const string CannotUpdateDeletedEntity =
        "A deleted entity cannot be updated.";

    public const string EntityAlreadyDeleted =
        "The entity has already been deleted.";
}
