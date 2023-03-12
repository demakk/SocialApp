using MediatR;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.Dal;
using Social.Domain.Aggregates.PostAggregates;
using Social.Domain.Exceptions;

namespace Social.Application.Posts.CommandHandlers;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, OperationResult<Post>>
{
    private readonly DataContext _ctx;

    public CreatePostCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = Post.CreatePost(request.UserProfileId, request.TextContent);
            _ctx.Posts.Add(post);
            await _ctx.SaveChangesAsync(cancellationToken);
            result.Payload = post;
        }
        catch (PostNotValidException exception)
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