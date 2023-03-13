using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.Commands;

public class AddPostInteraction : IRequest<OperationResult<PostInteraction>>
{
    public Guid UserProfileId { get; set; }
    public Guid PostId { get; set; }
    public InteractionType InteractionType { get; set; }
}