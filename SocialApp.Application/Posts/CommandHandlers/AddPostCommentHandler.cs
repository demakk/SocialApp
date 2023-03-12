using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;
using Social.Domain.Exceptions;

namespace Social.Application.Posts.CommandHandlers;

public class AddPostCommentHandler : IRequestHandler<AddPostComment, OperationResult<PostComment>>
{
    private readonly DataContext _ctx;

    public AddPostCommentHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<PostComment>> Handle(AddPostComment request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();
        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);
            if (post is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var comment = PostComment.CreatePostComment(request.PostId,
                request.Text, request.UserProfileId);
            
            post.AddPostComment(comment);
            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = comment;
        }
        catch (PostCommentNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e => result.AddError(ErrorCode.ValidationError, e));
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}