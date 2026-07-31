using WhoAmI.Application.Abstractions.Commands;

namespace WhoAmI.Application.Features.Profiles.UpdateSocialLinks;

public sealed record UpdateSocialLinksCommand(
    Guid Id,
    string LinkedIn,
    string GitHub
): ICommand;
