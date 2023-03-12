namespace Social.Application.Posts;

public static class PostErrorMessages
{
    public static string PostNotFound = "No post id {0} found";

    public static string PostDeleteNotPossible =
        "Impossible to delete the post because it's not the post owner that initiates the delete";

    public static string PostUpdateNotPossible =
        "Impossible to update the post because it's not the post owner that initiates the update";
}