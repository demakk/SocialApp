using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Identity.Commands;
using Social.Application.Models;
using Social.Application.Services;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.Identity.CommandHandlers;

public class LoginHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{

    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;


    //Options pattern. Map configuration info in appsettings to clr object
    public LoginHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {

        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
    }
    
    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var identity  = await _userManager.FindByEmailAsync(request.Username);

            var validationResult = await ValidateAndSetIdentityAsync(request, result, identity);
            
            if (!validationResult) return result;
            
            var profile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(i => i.IdentityId == identity.Id, cancellationToken);
            
            result.Payload = GetJwtString(identity, profile);
            return result;
        }
        catch (Exception e)
        {
            result.AddError(ErrorCode.UnknownError, e.Message);
        }

        return result;
    }

    private async Task<bool> ValidateAndSetIdentityAsync(LoginCommand request,
        OperationResult<string> result,  IdentityUser? identity)
    {
        if (identity is null)
        {
            result.AddError(ErrorCode.IdentityDoesNotExist, IdentityErrorMessages.NonExistentIdentityUser);
            return false;
        }

        var validPassword = await _userManager.CheckPasswordAsync(identity, request.Password);
        /*if (!validPassword)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.IdentityDoesNotExist,
                Message = "Provided password is incorrect"
            };
            result.Errors.Add(error);
            return false;
        }*/

        if (validPassword) return true;
        result.AddError(ErrorCode.IncorrectPassword, IdentityErrorMessages.IncorrectPassword);
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