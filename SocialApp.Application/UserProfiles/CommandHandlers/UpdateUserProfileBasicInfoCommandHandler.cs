using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Commands;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;
using Social.Domain.Exceptions;

namespace Social.Application.UserProfiles.CommandHandlers;

public class UpdateUserProfileBasicInfoCommandHandler : IRequestHandler<UpdateUserProfileBasicInfoCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public UpdateUserProfileBasicInfoCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfoCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        try
        {
            var userProfile = await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.Id == request.Id, cancellationToken: cancellationToken);

            if (userProfile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.Id));
                return result;
            }

            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
                request.Phone, request.DateOfBirth, request.CurrentCity);
        
            userProfile.UpdateBasicInfo(basicInfo);

            _ctx.UserProfiles.Update(userProfile);  
            await _ctx.SaveChangesAsync(cancellationToken);
            result.Payload = userProfile;
            return result;
        }
        catch (UserProfileNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e => { result.AddError(ErrorCode.ValidationError, e); });
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        return result;
    }
}