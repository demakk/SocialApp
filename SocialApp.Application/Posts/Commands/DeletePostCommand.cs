using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.Commands;

public class DeletePostCommand : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
}