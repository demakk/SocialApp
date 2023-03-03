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
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);
            if (post is null)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"No post id {request.PostId} found"
                };
                result.Errors.Add(error);
                return result;
            }

            var comment = PostComment.CreatePostComment(request.PostId,
                request.Text, request.UserProfileId);
            
            post.AddPostComment(comment);
            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync();

            result.Payload = comment;
        }
        catch (PostCommentNotValidException exception)
        {
            result.IsError = true;
            exception.ValidationErrors.ForEach(e =>
            {
                var error = new Error
                {
                    Code = ErrorCode.ValidationError, Message = e
                };

                result.Errors.Add(error);
            });
        }
        catch (Exception e)
        {
            result.IsError = true;

            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = e.Message
            };
            result.Errors.Add(error);
        }

        return result;
    }
}