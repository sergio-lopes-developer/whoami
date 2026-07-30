using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Testing.Factories;

public static class ProfileFactory {
    public static Profile Create() =>
        Profile.Create(
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
