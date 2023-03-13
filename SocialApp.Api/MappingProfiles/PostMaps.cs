using AutoMapper;
using Social.Domain.Aggregates.PostAggregates;
using SocialApp.Contracts.Posts.Responses;

namespace SocialApp.MappingProfiles;

public class PostMaps : Profile
{
    public PostMaps()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();
        CreateMap<PostInteraction, PostInteractionResponse>()
            .ForMember(dest => dest.Type, opt =>
                opt.MapFrom(src => src.InteractionType.ToString()))
            .ForMember(dest => dest.Author, opt =>
                opt.MapFrom(src => src.UserProfile));
    }
}