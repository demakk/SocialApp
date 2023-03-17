using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Social.Application.Identity.Commands;
using Social.Application.Identity.Queries;
using SocialApp.Contracts.Identity;
using SocialApp.Filters;

namespace SocialApp.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class IdentityController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IdentityController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] UserRegistration registration, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterIdentity>(registration);

        var response = await _mediator.Send(command, cancellationToken);
        if (response.IsError) return HandleErrorResponse(response.Errors);

        var userProfileContract = _mapper.Map<IdentityUserProfile>(response.Payload);
        
        return Ok(userProfileContract);
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] Login login, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var response = await _mediator.Send(command, cancellationToken);
       
        if (response.IsError) return HandleErrorResponse(response.Errors);

        var userProfileContract = _mapper.Map<IdentityUserProfile>(response.Payload);
        
        return Ok(userProfileContract);
    }

    [HttpDelete]
    [Route(ApiRoutes.Identity.IdentityById)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ValidateGuid("identityUserId")]
    public async Task<IActionResult> DeleteAccount(string identityUserid, CancellationToken cancellationToken)
    {
        var requesterId = HttpContext.GetIdentityIdClaimValue();

        var command = new RemoveAccountCommand
        {
            IdentityUserId = Guid.Parse(identityUserid),
            RequesterId = requesterId
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        return NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Identity.CurrentUser)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CurrentUser(CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var claimsPrinciple = HttpContext.User;
        
        var query = new GetCurrentUser
        {
            UserProfileId = userProfileId,
            ClaimsPrincipal = claimsPrinciple
        };

        var response = await _mediator.Send(query, cancellationToken);

        var profile = _mapper.Map<IdentityUserProfile>(response.Payload);

        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        return Ok(profile);
    }
}