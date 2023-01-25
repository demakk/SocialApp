using System.Net.Mime;
using System.Numerics;
using Social.Domain.Exceptions;
using Social.Domain.Validators.PostValidators;

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
    /// <summary>
    /// Creates post comment
    /// </summary>
    /// <param name="postId"> Id of the comment</param>
    /// <param name="text"> Text of the comment</param>
    /// <param name="userProfileId"> Id of the profile</param>
    /// <returns><see cref="PostComment"/></returns>
    /// <exception cref="PostCommentNotValidException">Thrown if the data provided for the post is not valid</exception>
    public static PostComment CreatePostComment(Guid postId, string text, Guid userProfileId)
    {
        var validator = new PostCommentValidator();

        var objectToValidate = new PostComment
        {
            PostId = postId,
            Text = text,
            UserProfileId = userProfileId,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
        
        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;

        var exception = new PostCommentNotValidException("Post comment is not valid ");

        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }

        throw exception;
    }
    
    //public methods

    public void UpdateCommentText(string text)
    {
        //add validation
        Text = text;
        LastModified = DateTime.UtcNow;
    }
}