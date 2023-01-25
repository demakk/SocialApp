using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Commands;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.CommandHandlers;

public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public DeleteUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        try
        {
            var profile = await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.Id == request.Id, cancellationToken: cancellationToken);
            if (profile is null)
            {
                result.IsError = true;
                var error = new Error{Code = ErrorCode.NotFound,
                    Message = $"No user with profile id {request.Id} found"};
                result.Errors.Add(error);   
                return result;
            }
        
            _ctx.UserProfiles.Remove(profile);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = profile;
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