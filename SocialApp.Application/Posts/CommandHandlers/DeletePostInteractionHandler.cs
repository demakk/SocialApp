using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.CommandHandlers;

public class DeletePostInteractionHandler : IRequestHandler<DeletePostInteraction, OperationResult<PostInteraction>>
{
    private readonly DataContext _ctx;

    public DeletePostInteractionHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<PostInteraction>> Handle(DeletePostInteraction request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostInteraction>();

        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Interactions)
                .FirstOrDefaultAsync(i => i.PostId == request.PostId, cancellationToken);

            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var interaction = post.Interactions.FirstOrDefault(i => i.Id == request.PostInteractionId);

            if (interaction is null)
            {
                result.AddError(ErrorCode.NotFound, PostErrorMessages.PostInteractionNotFound);
                return result;
            }

            if (interaction.UserProfileId != request.UserProfileId)
            {
                result.AddError(ErrorCode.InteractionRemovalNotAuthorized, PostErrorMessages.InteractionRemovalNotAuthorized);
                return result;
            }
            
            post.RemoveInteraction(interaction);

            _ctx.Posts.Update(post);

            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = interaction;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}