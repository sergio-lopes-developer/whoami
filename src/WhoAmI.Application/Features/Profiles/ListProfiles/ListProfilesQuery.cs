using WhoAmI.Application.Abstractions.Queries;

namespace WhoAmI.Application.Features.Profiles.ListProfiles;

public sealed record ListProfilesQuery :
    IQuery<IReadOnlyCollection<ListProfilesResponse>>;
