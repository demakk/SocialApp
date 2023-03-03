using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.Commands;

public class CreatePostCommand : IRequest<OperationResult<Post>>
{
    public Guid UserProfileId { get; set; }
    public string TextContent { get; set; }
}