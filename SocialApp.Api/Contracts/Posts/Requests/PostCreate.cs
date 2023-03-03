using System.ComponentModel.DataAnnotations;

namespace SocialApp.Contracts.Posts.Requests;

public class PostCreate
{
    [Required]
    public Guid UserProfileId { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string TextContent { get; set; }
}