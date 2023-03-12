using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Queries;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.QueryHandlers;

public class GetPostCommentsHandler : IRequestHandler<GetAllComments, OperationResult<List<PostComment>>>
{
    private readonly DataContext _ctx;

    public GetPostCommentsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<List<PostComment>>> Handle(GetAllComments request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostComment>>();

        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);
            
            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return result;
            }
            
            result.Payload = post.Comments.ToList();
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}