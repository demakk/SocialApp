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
                result.IsError = true;
                var error = new Error{Code = ErrorCode.NotFound,
                    Message = $"No post with profile id {request.Id} found"};
                result.Errors.Add(error);   
                return result;
            }
            result.Payload = post;
        }
        catch (Exception exception)
        {
            var error = new Error
            {
                Code = ErrorCode.UnknownError, Message = exception.Message
            };
            result.Errors.Add(error);
            result.IsError = true;
        }
        return result;
    }
}