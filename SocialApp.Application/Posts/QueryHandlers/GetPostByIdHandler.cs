using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Queries;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.QueryHandlers;

public class GetPostByIdHandler : IRequestHandler<GetPostById, OperationResult<Post>>
{
    private readonly DataContext _ctx;

    public GetPostByIdHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Post>> Handle(GetPostById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();

        try
        {
            var post = await _ctx.Posts
                .FirstOrDefaultAsync(post => post.PostId == request.Id, cancellationToken: cancellationToken);
            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostErrorMessages.PostNotFound, request.Id));
                return result;
            }
            result.Payload = post;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }
        return result;
    }
}