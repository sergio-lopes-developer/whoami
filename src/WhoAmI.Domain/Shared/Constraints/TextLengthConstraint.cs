namespace WhoAmI.Domain.Shared.Constraints;

public sealed record TextLengthConstraint(
    int MinLength,
    int MaxLength,
    Func<string?, bool> IsSatisfiedBy
);
