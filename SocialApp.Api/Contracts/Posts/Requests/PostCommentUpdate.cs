using System.ComponentModel.DataAnnotations;

namespace SocialApp.Contracts.Posts.Requests;

public class PostCommentUpdate
{
    [Required]
    public string Text { get; set; }
}