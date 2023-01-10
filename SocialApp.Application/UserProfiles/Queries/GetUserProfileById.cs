using MediatR;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.Queries;

public class GetUserProfileById : IRequest<UserProfile>
{
    public Guid Id { get; set; }
}