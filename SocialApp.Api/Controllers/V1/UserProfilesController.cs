﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Application.UserProfiles.Commands;
using Social.Application.UserProfiles.Queries;
using SocialApp.Contracts.UserProfileContracts.Requests;
using SocialApp.Contracts.UserProfileContracts.Responses;
using SocialApp.Filters;

namespace SocialApp.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        //throw new NotImplementedException("Method not implemented");
        var query = new GetAllUserProfiles();
        var response = await _mediator.Send(query);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
        return Ok(profiles);
    }
    
    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileById {Id = Guid.Parse(id)};
        var response = await _mediator.Send(query);

        if (response.IsError)
            return HandleErrorResponse(response.Errors);

        var profile = _mapper.Map<UserProfileResponse>(response.Payload);
        
        return Ok(profile);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateModel]
    public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UserProfileCreateUpdate profile)
    {
        var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(profile);
        command.Id = Guid.Parse(id);
        var response = await _mediator.Send(command);

        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }
    
}