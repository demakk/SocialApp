using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.Queries;

public class GetUserProfileById : IRequest<OperationResult<UserProfile>>
{
    public Guid Id { get; set; }
}