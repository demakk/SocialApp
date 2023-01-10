using MediatR;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.Queries;

public class GetAllUserProfiles : IRequest<IEnumerable<UserProfile>>
{
    
}