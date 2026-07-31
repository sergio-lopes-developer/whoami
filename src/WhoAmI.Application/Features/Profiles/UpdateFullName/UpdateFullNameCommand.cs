using WhoAmI.Application.Abstractions.Commands;

namespace WhoAmI.Application.Features.Profiles.UpdateFullName;

public sealed record UpdateFullNameCommand(
    Guid Id,
    string FirstName,
    string LastName
) : ICommand;
