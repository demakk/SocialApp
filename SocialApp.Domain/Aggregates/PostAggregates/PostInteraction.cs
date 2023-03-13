using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Domain.Aggregates.PostAggregates;

public class PostInteraction
{
    private PostInteraction()
    {
    }
    
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public InteractionType InteractionType { get; private set; }
    
    public UserProfile UserProfile { get; private set; }
    public Guid? UserProfileId { get; private set; }
    
    //Factories
    public static PostInteraction CreatePostInteraction(Guid userProfileId, Guid postId, InteractionType type)
    {
        return new PostInteraction
        {
            PostId = postId,
            InteractionType = type,
            UserProfileId = userProfileId
        };
    }


}