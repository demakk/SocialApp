using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.UserProfiles.Commands;
using Social.Dal;

namespace Social.Application.UserProfiles.CommandHandlers;

public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand>
{
    private DataContext _ctx;

    public DeleteUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.Id == request.Id);

        _ctx.UserProfiles.Remove(profile);
        await _ctx.SaveChangesAsync(cancellationToken);
        return new Unit();
    }
}