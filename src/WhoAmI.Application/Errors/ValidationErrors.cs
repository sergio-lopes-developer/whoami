using WhoAmI.Application.Results;

namespace WhoAmI.Application.Errors;

internal static class ValidationErrors {
    private static Dictionary<string, object?> FieldMetadata(string field) =>
        new() { ["Field"] = field };

    internal static Error Required(string field) =>
        new(
            "Validation.Required",
            $"{field} is required.",
            new Dictionary<string, object?> { ["Field"] = field }
        );

    internal static Error InvalidLength(string field, int min, int max) =>
        new(
            "Validation.InvalidLength",
            $"{field} must be between {min} and {max} characters.",
            new Dictionary<string, object?> {
                ["Field"] = field,
                ["Min"] = min,
                ["Max"] = max
            }
        );

    internal static Error InvalidLength(string field, int max) =>
        new(
            "Validation.InvalidLength",
            $"{field} must be lower than {max + 1} characters.",
            new Dictionary<string, object?> {
                ["Field"] = field,
                ["Max"] = max
            }
        );

    internal static Error InvalidFormat(string field) =>
        new(
            "Validation.InvalidFormat",
            $"{field} format is invalid.",
            new Dictionary<string, object?> { ["Field"] = field }
        );
}
