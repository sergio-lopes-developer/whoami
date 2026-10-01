using WhoAmI.Application.Abstractions.Commands;

namespace WhoAmI.Application.Features.Profiles.DeleteProfile;

public sealed record DeleteProfileCommand(Guid Id) : ICommand;
