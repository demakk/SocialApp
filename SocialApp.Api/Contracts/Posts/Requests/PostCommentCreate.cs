using System.ComponentModel.DataAnnotations;

namespace SocialApp.Contracts.Posts.Requests;

public class PostCommentCreate
{
    [Required]
    public string Text { get; set; }
    [Required]
    public string UserProfileId { get; set; }
}