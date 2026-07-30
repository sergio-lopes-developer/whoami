namespace WhoAmI.Application.Validation;

internal static class Ensure {
    internal static ValidationRuleBuilder<T> Field<T>(T value, string field) =>
        new(value, field);
}
