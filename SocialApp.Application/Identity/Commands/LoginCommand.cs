using MediatR;
using Social.Application.Models;

namespace Social.Application.Identity.Commands;

public class LoginCommand : IRequest<OperationResult<string>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}