using WhoAmI.Application.Results;

namespace WhoAmI.Application.Errors;

public static class PersistenceErrors {
    public static readonly Error UnmappedConstraint =
        new(
            "Persistence.UnmappedConstraint",
            "An unmapped database constraint violation occurred."
        );
}
