using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.CommandHandlers;

public class UpdatePostTextCommandHandler : IRequestHandler<UpdatePostTextCommand, OperationResult<Post>>
{
    private readonly DataContext _ctx;

    public UpdatePostTextCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Post>> Handle(UpdatePostTextCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();

        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);
            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(ErrorCode.PostUpdateNotPossible, PostErrorMessages.PostUpdateNotPossible);
                return result;
            }

            post.UpdatePostText(request.NewTextContent);
            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(cancellationToken);
            result.Payload = post;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        return result;
    }
}