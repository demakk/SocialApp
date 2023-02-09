using MediatR;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Commands;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;
using Social.Domain.Exceptions;

namespace Social.Application.UserProfiles.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;
    

    public CreateUserCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<UserProfile>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        try
        {
            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
                request.Phone, request.DateOfBirth, request.DateOfBirth, request.CurrentCity);

            var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);
            _ctx.Add(userProfile);
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
                    Code = ErrorCode.ValidationError, Message = e
                };

                result.Errors.Add(error);
            });
            return result;
        }
        catch (Exception exception)
        {
            var error = new Error
            {
                Code = ErrorCode.UnknownError, Message = exception.Message
            };
            result.Errors.Add(error);
            result.IsError = true;
            return result;
        }
    }
}