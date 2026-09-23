using WhoAmI.Application.Abstractions.Time;

namespace WhoAmI.Application.Time;

internal sealed class Clock : IClock {
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
