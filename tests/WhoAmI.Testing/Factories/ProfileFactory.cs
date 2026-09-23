using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Testing.Factories;

public static class ProfileFactory {
    private static readonly DateTimeOffset _defaultCreatedAt =
        new(2026, 9, 22, 14, 30, 0, TimeSpan.Zero);

    public static Profile Create(DateTimeOffset? createdAt = null) =>
        Profile.Create(
            createdAt ?? _defaultCreatedAt,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );

    public static Profile CreateWithoutEvents() {
        var profile = Create();
        profile.ClearDomainEvents();
        return profile;
    }
}
