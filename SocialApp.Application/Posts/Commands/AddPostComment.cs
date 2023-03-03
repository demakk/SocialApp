using MediatR;
using Social.Application.Models;
using Social.Application.UserProfiles.Queries;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.Commands;

public class AddPostComment : IRequest<OperationResult<PostComment>>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public string Text { get; set; }
}