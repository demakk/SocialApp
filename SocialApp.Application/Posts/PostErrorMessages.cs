namespace Social.Application.Posts;

public static class PostErrorMessages
{
    public static string PostNotFound = "No post id {0} found";

    public static string PostDeleteNotPossible =
        "Impossible to delete the post because it's not the post owner that initiates the delete";

    public static string PostUpdateNotPossible =
        "Impossible to update the post because it's not the post owner that initiates the update";

    public const string PostInteractionNotFound = "No interaction found";

    public const string PostCommentNotFound = "No post comment found";

    public const string InteractionRemovalNotAuthorized =
        "Cannot remove the interaction because you are not the author of this very interaction";
    
    public const string PostCommentUpdateNotPossible =
        "Impossible to delete the comment because it's not the comment's author";
}