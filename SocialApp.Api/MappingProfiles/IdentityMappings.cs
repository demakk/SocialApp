using AutoMapper;
using Social.Application.Identity.Commands;
using Social.Application.Identity.Commands.Dtos;
using SocialApp.Contracts.Identity;

namespace SocialApp.MappingProfiles;

public class IdentityMappings : Profile
{
    public IdentityMappings()
    {
        CreateMap<UserRegistration, RegisterIdentity>();
        CreateMap<Login, LoginCommand>();
        CreateMap<IdentityUserProfileDto, IdentityUserProfile>();
    }
}