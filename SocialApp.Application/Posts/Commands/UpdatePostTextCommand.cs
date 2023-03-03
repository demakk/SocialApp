using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.Commands;

public class UpdatePostTextCommand : IRequest<OperationResult<Post>>
{
    public string NewTextContent { get; set; }
    public Guid PostId { get; set; }
}