using System.Security.Claims;
using MediatR;
using Social.Application.Identity.Commands.Dtos;
using Social.Application.Models;

namespace Social.Application.Identity.Queries;

public class GetCurrentUser : IRequest<OperationResult<IdentityUserProfileDto>>
{
    public Guid UserProfileId { get; set; }
    public ClaimsPrincipal ClaimsPrincipal { get; set; }
}