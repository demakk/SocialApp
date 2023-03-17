using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.CommandHandlers;

public class UpdatePostCommentHandler : IRequestHandler<UpdatePostCommentCommand, OperationResult<PostComment>>
{

    private readonly DataContext _ctx;
    private readonly OperationResult<PostComment> _result;

    public UpdatePostCommentHandler(DataContext ctx)
    {
        _ctx = ctx;
        _result = new OperationResult<PostComment>();
    }

    public async Task<OperationResult<PostComment>> Handle(UpdatePostCommentCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);

            if (post is null)
            {
                _result.AddError(ErrorCode.UnknownError,
                    string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return _result;
            }

            var comment = post.Comments.FirstOrDefault(c => c.Id == request.CommentId);

            if (comment is null)
            {
                _result.AddError(ErrorCode.NotFound, PostErrorMessages.PostCommentNotFound);
                return _result;
            }

            if (comment.UserProfileId != request.UserProfileId)
            {
                _result.AddError(ErrorCode.UnauthorizedCommentRemoval,
                    PostErrorMessages.PostCommentUpdateNotPossible);
                return _result;
            }
            
            post.UpdatePostComment(comment.Id, request.Text);

            _ctx.Posts.Update(post);

            await _ctx.SaveChangesAsync(cancellationToken);

            _result.Payload = comment;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }
}