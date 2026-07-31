using WhoAmI.Application.Abstractions.Queries;

namespace WhoAmI.Application.Features.Profiles.GetProfileByEmail;

public sealed record GetProfileByEmailQuery(string Email) :
    IQuery<GetProfileByEmailResponse>;
