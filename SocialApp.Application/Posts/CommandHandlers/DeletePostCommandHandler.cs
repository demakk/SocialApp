using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Application.Posts.CommandHandlers;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, OperationResult<Post>>
{
    private readonly DataContext _ctx;

    public DeletePostCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);
            if (post is null)
            {
                result.IsError = true;
                var error = new Error{Code = ErrorCode.NotFound,
                    Message = $"No post with id {request.PostId} found"};
                result.Errors.Add(error);   
                return result;
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.IsError = true;
                var error = new Error{Code = ErrorCode.PostDeleteNotPossible,
                    Message = $"Impossible to delete the post because it's not the post owner that initiates the delete"};
                result.Errors.Add(error);
                return result;
            }
            _ctx.Posts.Remove(post);
            await _ctx.SaveChangesAsync();
            
            result.Payload = post;
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