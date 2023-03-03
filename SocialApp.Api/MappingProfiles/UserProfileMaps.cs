using AutoMapper;
using Social.Application.UserProfiles.Commands;
using Social.Domain.Aggregates.UserProfileAggregates;
using SocialApp.Contracts.UserProfileContracts.Requests;
using SocialApp.Contracts.UserProfileContracts.Responses;

namespace SocialApp.MappingProfiles;

public class UserProfileMaps : Profile
{
    public UserProfileMaps()
    {
        CreateMap<UserProfile, UserProfileResponse>();
        CreateMap<BasicInfo, BasicInfoResponse>();
        CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfoCommand>();
    }
}