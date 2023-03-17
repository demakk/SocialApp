using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Identity.Commands;
using Social.Application.Identity.Commands.Dtos;
using Social.Application.Models;
using Social.Application.Services;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.Identity.CommandHandlers;

public class LoginHandler : IRequestHandler<LoginCommand, OperationResult<IdentityUserProfileDto>>
{

    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private readonly IMapper _mapper;
    private OperationResult<IdentityUserProfileDto> _result = new();


    //Options pattern. Map configuration info in appsettings to clr object
    public LoginHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService, IMapper mapper)
    {

        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
    }
    
    public async Task<OperationResult<IdentityUserProfileDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var identity  = await _userManager.FindByEmailAsync(request.Username);

            var validationResult = await ValidateAndSetIdentityAsync(request, identity);
            
            if (!validationResult) return _result;
            
            var profile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(i => i.IdentityId == identity.Id, cancellationToken);

            _result.Payload = _mapper.Map<IdentityUserProfileDto>(profile);
            _result.Payload.Token = GetJwtString(identity, profile);
            _result.Payload.UserName = identity.UserName;
            return _result;
        }
        catch (Exception e)
        {
            _result.AddError(ErrorCode.UnknownError, e.Message);
        }

        return _result;
    }

    private async Task<bool> ValidateAndSetIdentityAsync(LoginCommand request, IdentityUser? identity)
    {
        if (identity is null)
        {
            _result.AddError(ErrorCode.IdentityDoesNotExist, IdentityErrorMessages.NonExistentIdentityUser);
            return false;
        }

        var validPassword = await _userManager.CheckPasswordAsync(identity, request.Password);
        
        if (validPassword) return true;
        _result.AddError(ErrorCode.IncorrectPassword, IdentityErrorMessages.IncorrectPassword);
        return false;
    }
    
    private string GetJwtString(IdentityUser identity, UserProfile profile)
    {
        var claimsIdentity = new ClaimsIdentity(new ClaimsIdentity(new Claim[]
        {
            new (JwtRegisteredClaimNames.Sub, identity.Email),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Email, identity.Email),
            new ("IdentityId", identity.Id),
            new ("UserProfileId", profile.Id.ToString())
        }));
            
        var securityToken = _identityService.CreateSecurityToken(claimsIdentity);
        return _identityService.WriteToken(securityToken);
    }
}