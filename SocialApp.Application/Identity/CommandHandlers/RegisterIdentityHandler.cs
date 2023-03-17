using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Social.Application.Enums;
using Social.Application.Identity.Commands;
using Social.Application.Identity.Commands.Dtos;
using Social.Application.Models;
using Social.Application.Services;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;
using Social.Domain.Exceptions;

namespace Social.Application.Identity.CommandHandlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<IdentityUserProfileDto>>
{

    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private readonly IMapper _mapper;
    private OperationResult<IdentityUserProfileDto> _result = new ();

    public RegisterIdentityHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService, IMapper mapper)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
    }
    
    public async Task<OperationResult<IdentityUserProfileDto>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
    {
        try
        {
            var creationValidated = await ValidateIdentityExistsAsync(request);

            if (!creationValidated) return _result;
            
            var identity = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username
            };

             await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);

            var userCreationSuccessful = await CreateIdentityUserAsync(identity, request, transaction, cancellationToken);

            if (!userCreationSuccessful) return _result;
            
            var profile = await CreateUserProfileAsync(identity, request, transaction, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            _result.Payload = _mapper.Map<IdentityUserProfileDto>(profile);
            _result.Payload.Token = GetJwtString(identity, profile);
            _result.Payload.UserName = identity.UserName;
            return _result;

        }
        catch (UserProfileNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e => { _result.AddError(ErrorCode.ValidationError, e); });
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task<bool> ValidateIdentityExistsAsync(RegisterIdentity request)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);
        if (existingIdentity == null) return true;
        _result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessages.IdentityUserAlreadyExists);
        return false;
    }

    private async Task<bool> CreateIdentityUserAsync(IdentityUser identity,
        RegisterIdentity request, IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        var identityResult = await _userManager.CreateAsync(identity, request.Password);
        if (identityResult.Succeeded) return true;
        
        foreach (var identityError in identityResult.Errors)
        {
            _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
        }
        await transaction.RollbackAsync(cancellationToken);
        return false;
    }

    private async Task<UserProfile> CreateUserProfileAsync(IdentityUser identity, RegisterIdentity request,
        IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        
        try
        {
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                request.Username, request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            _ctx.UserProfiles.Add(profile);
            await _ctx.SaveChangesAsync(cancellationToken);
            return profile;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
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