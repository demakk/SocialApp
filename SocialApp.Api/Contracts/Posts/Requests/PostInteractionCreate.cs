using System.ComponentModel.DataAnnotations;
using Social.Domain.Aggregates.PostAggregates;

namespace SocialApp.Contracts.Posts.Requests;

public class PostInteractionCreate
{
    [Required]
    public InteractionType InteractionType { get; set; }
}