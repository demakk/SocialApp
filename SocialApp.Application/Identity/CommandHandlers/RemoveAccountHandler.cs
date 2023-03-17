using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Identity.Commands;
using Social.Application.Models;
using Social.Application.UserProfiles;
using Social.Dal;

namespace Social.Application.Identity.CommandHandlers;

public class RemoveAccountHandler : IRequestHandler<RemoveAccountCommand, OperationResult<bool>>
{
    private readonly DataContext _ctx;

    public RemoveAccountHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<bool>> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();

        try
        {
            var identityUser = await _ctx.Users.FirstOrDefaultAsync(u
                => u.Id == request.IdentityUserId.ToString(), cancellationToken: cancellationToken);

            if (identityUser is null)
            {
                result.AddError(ErrorCode.IdentityDoesNotExist, IdentityErrorMessages.NonExistentIdentityUser);
                return result;
            }

            var userProfile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(p => p.IdentityId == request.IdentityUserId.ToString(), cancellationToken);

            if (userProfile is null)
            {
                result.AddError(ErrorCode.NotFound, UserProfilesErrorMessages.UserProfileNotFound);
                return result;
            }

            if (identityUser.Id != request.RequesterId.ToString())
            {
                result.AddError(ErrorCode.UnauthorizedAccountRemoval, IdentityErrorMessages.UnauthorizedAccountRemoval);
                return result;
            }

            
            await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _ctx.UserProfiles.Remove(userProfile);
                _ctx.Users.Remove(identityUser);
                await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            result.Payload = true;
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}