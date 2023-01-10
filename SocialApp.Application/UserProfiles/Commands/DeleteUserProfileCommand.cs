using MediatR;

namespace Social.Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest
{
    public Guid Id { get; set; }
}