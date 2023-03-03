using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Social.Application.Enums;
using Social.Application.Identity.Commands;
using Social.Application.Models;
using Social.Application.Services;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;
using Social.Domain.Exceptions;

namespace Social.Application.Identity.CommandHandlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<string>>
{

    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public RegisterIdentityHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
    }
    
    public async Task<OperationResult<string>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var creationValidated = await ValidateIdentityExistsAsync(result, request);

            if (!creationValidated) return result;
            
            var identity = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username
            };

             await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);

            var userCreationSuccessful = await CreateIdentityUserAsync(result, identity, request, transaction);

            if (!userCreationSuccessful) return result;
            
            var profile = await CreateUserProfileAsync(identity, request, transaction);
            await transaction.CommitAsync();
            
            result.Payload = GetJwtString(identity, profile);
            return result;

        }
        catch (UserProfileNotValidException exception)
        {
            result.IsError = true;
            exception.ValidationErrors.ForEach(e =>
            {
                var error = new Error
                {
                    Code = ErrorCode.ValidationError, Message = e
                };

                result.Errors.Add(error);
            });
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

    private async Task<bool> ValidateIdentityExistsAsync(OperationResult<string> result,
        RegisterIdentity request)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);
        if (existingIdentity != null)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.IdentityUserAlreadyExists,
                Message = $"Provided address {request.Username} already exists. Cannot register new user"
            };
            result.Errors.Add(error);
            return false;
        }

        return true;
    }

    private async Task<bool> CreateIdentityUserAsync(OperationResult<string> result,
        IdentityUser identity, RegisterIdentity request, IDbContextTransaction transaction)
    {
        var identityResult = await _userManager.CreateAsync(identity, request.Password);
        if (!identityResult.Succeeded)
        {
                
            foreach (var identityError in identityResult.Errors)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.IdentityCreationFailed,
                    Message = identityError.Description
                };
                result.Errors.Add(error);
                await transaction.RollbackAsync();
            }

            return false;
        }
        return true;
    }

    private async Task<UserProfile> CreateUserProfileAsync(IdentityUser identity, RegisterIdentity request,
        IDbContextTransaction transaction)
    {
        
        try
        {
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                request.Username, request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            _ctx.UserProfiles.Add(profile);
            await _ctx.SaveChangesAsync();
            return profile;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private string GetJwtString(IdentityUser identity, UserProfile profile)
    {            var claimsIdentity = new ClaimsIdentity(new ClaimsIdentity(new Claim[]
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