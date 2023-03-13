using AutoMapper;
using Social.Application.UserProfiles.Commands;
using Social.Domain.Aggregates.UserProfileAggregates;
using SocialApp.Contracts.Posts.Responses;
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
        CreateMap<UserProfile, InteractionUser>()
            .ForMember(dest => dest.FullName, opt
                => opt.MapFrom(src => src.BasicInfo.FirstName+ src.BasicInfo.LastName))
            .ForMember(dest => dest.CurrentCity, opt 
                => opt.MapFrom(src => src.BasicInfo.CurrentCity));
    }
}