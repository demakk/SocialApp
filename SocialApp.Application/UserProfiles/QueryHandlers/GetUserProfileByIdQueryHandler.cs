using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.UserProfiles.Queries;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileById, UserProfile>
{
    private readonly DataContext _ctx;

    public GetUserProfileByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<UserProfile> Handle(GetUserProfileById request, CancellationToken cancellationToken)
    {
        return await _ctx.UserProfiles.FirstOrDefaultAsync(p => p.Id == request.Id);
    }
}