using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest<OperationResult<UserProfile>>
{
    public Guid Id { get; set; }
}