using AutoMapper;
using Social.Application.UserProfiles.Commands;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.AutomapperProfiles;

internal class UserProfileMap : Profile
{
    public UserProfileMap()
    {
        //CreateMap<CreateUserCommand, BasicInfo>();
    }
}