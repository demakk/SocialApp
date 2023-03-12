using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Queries;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileById, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public GetUserProfileByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        try
        {
            var profile = await _ctx.UserProfiles.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
            if (profile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.Id));
                return result;
            }

            result.Payload = profile;
            return result;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
            return result;
        }
    }
}