using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Queries;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.QueryHandlers;

public class GetPostInteractionsHandler : IRequestHandler<GetPostInteractions, OperationResult<List<PostInteraction>>>
{

    private readonly DataContext _ctx;

    public GetPostInteractionsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractions request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostInteraction>>();
        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Interactions)
                .Include(i => i.UserProfile)
                . FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            result.Payload = post.Interactions.ToList();

        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}