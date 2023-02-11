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
    }
}