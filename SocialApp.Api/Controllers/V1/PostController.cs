using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Application.Posts.Commands;
using Social.Application.Posts.Queries;
using SocialApp.Contracts.Common;
using SocialApp.Contracts.Posts.Requests;
using SocialApp.Contracts.Posts.Responses;
using SocialApp.Filters;


namespace SocialApp.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PostController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public PostController(IMapper mapper, IMediator mediator)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var query = new GetAllPosts();
        var response = await _mediator.Send(query);
        var posts = _mapper.Map<List<PostResponse>>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(posts);
    }

    
    [HttpGet]
    [Route(ApiRoutes.Post.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetPostById(string id)
    {
        var query = new GetPostById(){Id = Guid.Parse(id)};
        var response = await _mediator.Send(query);
        var post = _mapper.Map<PostResponse>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(post);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreatePost([FromBody] PostCreate post)
    {

        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        
        var command = new CreatePostCommand
        {
            UserProfileId = userProfileId,
            TextContent = post.TextContent
        };

        var response = await _mediator.Send(command);

        var mapped = _mapper.Map<PostResponse>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) 
            : CreatedAtAction(nameof(GetPostById), new {id = response.Payload.UserProfileId},
                mapped);
    }

    
    [HttpPatch]
    [Route(ApiRoutes.Post.IdRoute)]
    [ValidateGuid("id")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePost(string id, [FromBody] PostUpdate post)
    {
        
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        
        var command = new UpdatePostTextCommand
        {
            PostId = Guid.Parse(id),
            NewTextContent = post.TextContent,
            UserProfileId = userProfileId
        };

        var response = await _mediator.Send(command);
        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
        
    }

    [HttpDelete]
    [Route(ApiRoutes.Post.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new DeletePostCommand
        {
            PostId = Guid.Parse(id),
            UserProfileId = userProfileId
        };

        var response = await _mediator.Send(command);
        
        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Post.PostComments)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetCommentsByPostId(string postId)
    {
        var query = new GetAllComments {PostId = Guid.Parse(postId)};

        var response = await _mediator.Send(query);

        if (response.IsError) HandleErrorResponse(response.Errors);
        
        var comments = _mapper.Map<List<PostCommentResponse>>(response.Payload);
        
        return Ok(comments);
    }


    [Route(ApiRoutes.Post.PostComments)]
    [ValidateGuid("postId")]
    [ValidateModel]
    [HttpPost]
    public async Task<IActionResult> AddPostComment(string postId, [FromBody] PostCommentCreate comment)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new AddPostComment
        {
            PostId = Guid.Parse(postId),
            Text = comment.Text,
            UserProfileId = userProfileId
        };

        var response = await _mediator.Send(command);
        if (response.IsError) HandleErrorResponse(response.Errors);
        
        var mappedComment = _mapper.Map<PostCommentResponse>(response.Payload);
        
        return Ok(mappedComment);
    }

    
    [HttpPut]
    [Route(ApiRoutes.Post.CommentById)]
    [ValidateGuid("postId", "commentId")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePostComment(string postId, string commentId,
        [FromBody] PostCommentUpdate comment, CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new UpdatePostCommentCommand
        {
            UserProfileId = userProfileId,
            CommentId = Guid.Parse(commentId),
            PostId = Guid.Parse(postId),
            Text = comment.Text
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) return HandleErrorResponse(response.Errors);

        return NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.Post.CommentById)]
    [ValidateGuid("postId", "commentId")]
    [ValidateModel]
    public async Task<IActionResult> RemoveCommentFromPost(string postId, string commentId,
        CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new DeletePostCommentCommand
        {
            UserProfileId = userProfileId,
            CommentId = Guid.Parse(commentId),
            PostId = Guid.Parse(postId)
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) HandleErrorResponse(response.Errors);
        
        return NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Post.PostInteractions)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetPostInteractions(string postId, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(postId);

        var query = new GetPostInteractions {PostId = id};

        var response = await _mediator.Send(query, cancellationToken);

        var mapped = _mapper.Map<List<PostInteractionResponse>>(response.Payload);
        
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(mapped);
    }

    [HttpPost]
    [Route(ApiRoutes.Post.PostInteractions)]
    [ValidateGuid("postId")]
    [ValidateModel]
    public async Task<IActionResult> AddPostInteraction(string postId,
        [FromBody] PostInteractionCreate interaction, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(postId);
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new AddPostInteraction
        {
            UserProfileId = userProfileId,
            PostId = id,
            InteractionType = interaction.InteractionType
        };

        var response = await _mediator.Send(command, cancellationToken);

        var mapped = _mapper.Map<PostInteractionResponse>(response.Payload);
        
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(mapped);
    }


    [HttpDelete]
    [Route(ApiRoutes.Post.InteractionById)]
    [ValidateGuid("postId", "interactionId")]
    public async Task<IActionResult> RemovePostInteraction(string postId, string interactionId,
        CancellationToken cancellationToken)
    {
        var command = new DeletePostInteraction
        {
            PostId = Guid.Parse(postId),
            PostInteractionId = Guid.Parse(interactionId),
            UserProfileId = HttpContext.GetUserProfileIdClaimValue()
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) return HandleErrorResponse(response.Errors);

        var mapped = _mapper.Map<PostInteractionResponse>(response.Payload);

        return Ok(mapped);
    }

}