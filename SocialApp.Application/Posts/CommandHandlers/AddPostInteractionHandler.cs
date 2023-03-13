using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.CommandHandlers;

public class AddPostInteractionHandler : IRequestHandler<AddPostInteraction, OperationResult<PostInteraction>>
{
    private readonly DataContext _ctx;

    public AddPostInteractionHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<PostInteraction>> Handle(AddPostInteraction request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostInteraction>();
        try
        {
            var post = await _ctx.Posts.Include(p => p.Interactions)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var postInteraction = PostInteraction.CreatePostInteraction(request.UserProfileId,
                request.PostId, request.InteractionType);
            
            post.AddInteraction(postInteraction);

            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = postInteraction;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}