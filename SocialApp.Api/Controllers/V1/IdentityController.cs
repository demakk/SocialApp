using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Social.Application.Identity.Commands;
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
    public async Task<IActionResult> Register([FromBody] UserRegistration registration)
    {
        var command = _mapper.Map<RegisterIdentity>(registration);

        var result = await _mediator.Send(command);
        if (result.IsError) return HandleErrorResponse(result.Errors);

        var authenticationResult = new AuthenticationResult { Token = result.Payload };
        
        return Ok(authenticationResult);
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var response = await _mediator.Send(command);
       
        if (response.IsError) return HandleErrorResponse(response.Errors);

        var authenticationResult = new AuthenticationResult { Token = response.Payload };
        
        return Ok(authenticationResult);

    }
}