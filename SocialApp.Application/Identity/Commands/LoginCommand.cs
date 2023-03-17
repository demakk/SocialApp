using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Social.Application.Identity.Commands.Dtos;
using Social.Application.Models;

namespace Social.Application.Identity.Commands;

public class LoginCommand : IRequest<OperationResult<IdentityUserProfileDto>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}