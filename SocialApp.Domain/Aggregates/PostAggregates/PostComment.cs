using System.Net.Mime;
using System.Numerics;

namespace Social.Domain.Aggregates.PostAggregates;

public class PostComment
{
    private PostComment()
    {
        
    }
    
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public string Text { get; private set; }
    public Guid UserProfileId { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }
    
    //Factories
    public static PostComment CreatePostComment(Guid postId, string text, Guid userProfileId)
    {
        //TO DO: validation
        return new PostComment
        {
            PostId = postId,
            Text = text,
            UserProfileId = userProfileId,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }
    
    //public methods

    public void UpdateCommentText(string text)
    {
        Text = text;
        LastModified = DateTime.UtcNow;
    }
}