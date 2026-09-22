namespace WhoAmI.Application.Abstractions.Time;

internal interface IClock {
    DateTimeOffset UtcNow { get; }
}
