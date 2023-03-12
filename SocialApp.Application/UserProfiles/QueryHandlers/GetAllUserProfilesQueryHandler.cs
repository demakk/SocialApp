using System.Collections;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Queries;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.QueryHandlers;

internal class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfiles, OperationResult<IEnumerable<UserProfile>>>
{
    private readonly DataContext _ctx;

    public GetAllUserProfilesQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<IEnumerable<UserProfile>>();
        try
        {
            var profiles = await _ctx.UserProfiles.ToListAsync(cancellationToken: cancellationToken);
            result.Payload = profiles;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}