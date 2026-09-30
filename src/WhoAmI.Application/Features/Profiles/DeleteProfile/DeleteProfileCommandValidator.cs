using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;
using WhoAmI.Application.Validation;

namespace WhoAmI.Application.Features.Profiles.DeleteProfile;

internal sealed class DeleteProfileCommandValidator :
    ICommandValidator<DeleteProfileCommand>
{
    public IReadOnlyCollection<Error> Validate(DeleteProfileCommand command) {
        var errors = new List<Error>();

        Ensure.Field(command.Id, nameof(command.Id))
            .IsRequired()
            .AddTo(errors);

        return errors;
    }
}
