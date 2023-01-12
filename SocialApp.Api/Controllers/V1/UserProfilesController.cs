using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Social.Application.Enums;
using Social.Application.UserProfiles.Commands;
using Social.Application.UserProfiles.Queries;
using SocialApp.Contracts.Common;
using SocialApp.Contracts.UserProfileContracts.Requests;
using SocialApp.Contracts.UserProfileContracts.Responses;

namespace SocialApp.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class UserProfilesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public UserProfilesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        var query = new GetAllUserProfiles();
        var response = await _mediator.Send(query);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response);
        return Ok(profiles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileCreateUpdate profile)
    {
        var command = _mapper.Map<CreateUserCommand>(profile);
        var response = await _mediator.Send(command);
        
        var responseProfile = _mapper.Map<UserProfileResponse>(response);
        return CreatedAtAction(nameof(GetUserProfileById), new {id = response.Id }, responseProfile);
    }

    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileById {Id = Guid.Parse(id)};
        var response = await _mediator.Send(query);

        if (response is null) return NotFound($"No user with profile id {id} found");
        var profile = _mapper.Map<UserProfileResponse>(response);
        return Ok(profile);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UserProfileCreateUpdate profile)
    {
        var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(profile);
        command.Id = Guid.Parse(id);
        var response = await _mediator.Send(command);

        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> DeleteUserProfile(string id)
    {
        var command = new DeleteUserProfileCommand
        {
            Id = Guid.Parse(id)
        };
        var response = await _mediator.Send(command);
        return NoContent();
    }
}