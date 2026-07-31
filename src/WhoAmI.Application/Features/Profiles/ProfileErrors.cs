using WhoAmI.Application.Results;

namespace WhoAmI.Application.Features.Profiles;

public static class ProfileErrors {
    public static Error NotFoundById(Guid id) =>
        new(
            "Profile.NotFound",
            "Profile was not found.",
            new Dictionary<string, object?> { ["ProfileId"] = id }
        );
}
