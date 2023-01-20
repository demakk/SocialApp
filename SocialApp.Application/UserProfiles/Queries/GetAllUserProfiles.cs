using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.Queries;

public class GetAllUserProfiles : IRequest<OperationResult<IEnumerable<UserProfile>>>
{
}