using Social.Domain.Aggregates.PostAggregates;

namespace SocialApp.Contracts.Posts.Responses;

public class PostInteractionResponse
{
    
    public Guid Id { get; set; }
    public string Type { get; set; }
    public InteractionUser Author { get; set; }
    
    
}