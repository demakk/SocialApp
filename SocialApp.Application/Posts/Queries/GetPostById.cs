using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.Queries;

public class GetPostById : IRequest<OperationResult<Post>>
{
    public Guid Id { get; set; }
}