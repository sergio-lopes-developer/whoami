using WhoAmI.Application.Abstractions.Commands;

namespace WhoAmI.Application.Features.Profiles.UpdateEmail;

public sealed record UpdateEmailCommand(Guid Id, string Email) : ICommand;
