using MediatR;
using Social.Application.UserProfiles.Commands;
using Social.Dal;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserProfile>
{
    private readonly DataContext _ctx;
    

    public CreateUserCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<UserProfile> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
            request.Phone, request.DateOfBirth, request.DateOfBirth, request.CurrentCity);

        var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);
        _ctx.Add(userProfile);
        await _ctx.SaveChangesAsync(cancellationToken);

        return userProfile;
    }
}