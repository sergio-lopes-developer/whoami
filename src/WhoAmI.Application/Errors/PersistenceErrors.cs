using WhoAmI.Application.Results;

namespace WhoAmI.Application.Errors;

public static class PersistenceErrors {
    public static readonly Error UnmappedViolation =
        new(
            "Persistence.UnmappedViolation",
            "An unmapped database constraint violation occurred."
        );
}
