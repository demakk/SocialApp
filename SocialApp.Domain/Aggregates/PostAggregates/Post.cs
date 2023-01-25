using System.ComponentModel.DataAnnotations;
using Social.Domain.Aggregates.UserProfileAggregates;
using Social.Domain.Exceptions;
using Social.Domain.Validators.PostValidators;

namespace Social.Domain.Aggregates.PostAggregates;

public class Post
{
    private readonly List<PostComment> _comments = new();
    private readonly List<PostInteraction> _interactions = new();
    private Post()
    {
    }
    
    public Guid PostId { get; private set; }
    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }  
    
    public string TextContent { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }

    //getters
    public IEnumerable<PostComment> Comments => _comments;

    public IEnumerable<PostInteraction> Interactions => _interactions;


    //factory method
    public static Post CreatePost(Guid userProfileId, string textContent)
    {
        var validator = new PostValidator();
        
        var postToValidate = new Post
        {
            UserProfileId = userProfileId,
            TextContent = textContent,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        var validationResult = validator.Validate(postToValidate);

        if (validationResult.IsValid) return postToValidate;

        var exception = new PostNotValidException("The post is not valid");

        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }

        throw exception;
    }
    
    //public methods
    /// <summary>
    /// Updates the post's text
    /// </summary>
    /// <param name="newText">The updated post text</param>
    /// <exception cref="PostNotValidException"></exception>
    public void UpdatePostText(string newText)
    {
        if (string.IsNullOrWhiteSpace(newText))
        {
            var exception = new PostNotValidException("Post is not valid" +
                                                      "Post text is not valid");
            exception.ValidationErrors.Add("The post text cannot be null or contain only a whitespace");
            throw exception;
        }
        TextContent = newText;
        LastModified = DateTime.UtcNow;
    }

    public void AddPostComment(PostComment newComment)
    {
        _comments.Add(newComment);
    }

    public void RemoveComment(PostComment toRemove)
    {
        _comments.Remove(toRemove);
    }

    public void AddInteraction(PostInteraction newInteraction)
    {
        _interactions.Add(newInteraction);
    }

    public void RemoveInteraction(PostInteraction toRemove)
    {
        _interactions.Remove(toRemove);
    }

}