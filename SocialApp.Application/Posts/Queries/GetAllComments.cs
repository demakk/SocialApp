using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.Queries;

public class GetAllComments : IRequest<OperationResult<List<PostComment>>>
{
    public Guid PostId { get; set; }
}