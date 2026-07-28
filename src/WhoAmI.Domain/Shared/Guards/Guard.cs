using WhoAmI.Domain.Shared.Exceptions;

namespace WhoAmI.Domain.Shared.Guards;

internal static class Guard {
    internal static void Against(bool condition, string message) {
        if (condition) throw new DomainException(message);
    }

    internal static void Against<T>(
        bool condition,
        Func<T> exceptionFactory
    ) where T : DomainException {
        if (condition) throw exceptionFactory();
    }

    internal static void AgainstEmptyGuid(
        Guid id,
        string paramName,
        string? message = null
    ) {
        if (id == Guid.Empty) {
            throw new DomainException(
                message ?? $"{paramName} cannot be Guid.Empty."
            );
        }
    }

    internal static void AgainstNull(
        object? value,
        string paramName,
        string? message = null
    ) {
        if (value is null) {
            throw new DomainException(
                message ?? $"{paramName} cannot be null."
            );
        }
    }

    internal static void AgainstNullOrEmpty<T>(
        IReadOnlyCollection<T>? value,
        string paramName,
        string? message = null
    ) {
        if (value is null || value.Count == 0) {
            throw new DomainException(
                message ?? $"{paramName} cannot be null or empty."
            );
        }
    }

    internal static void AgainstNullOrWhiteSpace(
        string? value,
        string paramName,
        string? message = null
    ) {
        if (string.IsNullOrWhiteSpace(value)) {
            throw new DomainException(
                message ?? $"{paramName} cannot be null, empty, or whitespace."
            );
        }
    }
}
