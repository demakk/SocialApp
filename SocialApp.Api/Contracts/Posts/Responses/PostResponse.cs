namespace SocialApp.Contracts.Posts.Responses;

public class PostResponse
{
    public Guid UserProfileId { get;  set; }
    public string TextContent { get;  set; }
    public DateTime CreatedDate { get;  set; }
    public DateTime LastModified { get;  set; }
}