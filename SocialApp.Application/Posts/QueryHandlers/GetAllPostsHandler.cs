using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Queries;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.QueryHandlers;

public class GetAllPostsHandler : IRequestHandler<GetAllPosts, OperationResult<List<Post>>>
{
    private DataContext _ctx;

    public GetAllPostsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<List<Post>>> Handle(GetAllPosts request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<Post>>();
        try
        {
            var posts = await _ctx.Posts.ToListAsync(cancellationToken: cancellationToken);

            result.Payload = posts;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }
        
        return result;

    }
}