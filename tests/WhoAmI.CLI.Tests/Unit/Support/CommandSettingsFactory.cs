using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Unit.Support;

internal static class CommandSettingsFactory {
    public static CreateProfileCommandSettings CreateProfile() => new() {
        FirstName = "Sérgio",
        LastName = "Lopes",
        Email = "email@provider.com",
        LinkedIn = "https://www.linkedin.com/in/username",
        GitHub = "https://github.com/username"
    };

    public static UpdateEmailCommandSettings UpdateEmail() => new() {
        Id = Guid.NewGuid(),
        Email = "john.doe@email.com"
    };

    public static UpdateFullNameCommandSettings UpdateFullName() => new() {
        Id = Guid.NewGuid(),
        FirstName = "Sérgio",
        LastName = "Lopes"
    };

    public static UpdateSocialLinksCommandSettings UpdateSocialLinks() =>
        new() {
            Id = Guid.NewGuid(),
            LinkedIn = "https://www.linkedin.com/in/username",
            GitHub = "https://github.com/username"
        };
}
