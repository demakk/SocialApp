using MediatR;
using Social.Application.Models;

namespace Social.Application.Identity.Commands;

public class RemoveAccountCommand : IRequest<OperationResult<bool>>
{
    public Guid IdentityUserId { get; set; }

    public Guid RequesterId { get; set; }
}