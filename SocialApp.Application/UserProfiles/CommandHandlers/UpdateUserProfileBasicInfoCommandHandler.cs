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
                result.IsError = true;
                var error = new Error{Code = ErrorCode.NotFound,
                    Message = $"No user with profile id {request.Id} found"};
                result.Errors.Add(error);   
                return result;
            }

            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
                request.Phone, request.DateOfBirth, request.DateOfBirth, request.CurrentCity);
        
            userProfile.UpdateBasicInfo(basicInfo);

            _ctx.UserProfiles.Update(userProfile);  
            await _ctx.SaveChangesAsync(cancellationToken);
            result.Payload = userProfile;
            return result;
        }
        catch (UserProfileNotValidException exception)
        {
            result.IsError = true;
            exception.ValidationErrors.ForEach(e =>
            {
                var error = new Error
                {
                    Code = ErrorCode.ValidationError, Message = exception.Message
                };

                result.Errors.Add(error);
            });
            return result;
        }
        catch (Exception e)
        {
            var error = new Error { Code = ErrorCode.ServerError, Message = e.Message }; 
            result.IsError = true;
            result.Errors.Add(error);
        }
        return result;
    }
}