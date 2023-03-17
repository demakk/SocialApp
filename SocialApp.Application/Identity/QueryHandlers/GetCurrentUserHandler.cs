using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Identity.Commands.Dtos;
using Social.Application.Identity.Queries;
using Social.Application.Models;
using Social.Dal;

namespace Social.Application.Identity.QueryHandlers;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUser, OperationResult<IdentityUserProfileDto>>
{

    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private OperationResult<IdentityUserProfileDto> _result = new();

    public GetCurrentUserHandler(DataContext ctx, UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _ctx = ctx;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<OperationResult<IdentityUserProfileDto>> Handle(GetCurrentUser request,
        CancellationToken cancellationToken)
    {
        try
        {
            var identity = await _userManager.GetUserAsync(request.ClaimsPrincipal);

            if (identity is null)
            {
                _result.AddError(ErrorCode.NotFound, IdentityErrorMessages.NonExistentIdentityUser);
                return _result;
            }

            var profile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(p => p.Id == request.UserProfileId, cancellationToken);

            _result.Payload = _mapper.Map<IdentityUserProfileDto>(profile);
            _result.Payload.UserName = identity.UserName;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }
        return _result;
    }
}