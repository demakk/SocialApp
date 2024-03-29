﻿namespace SocialApp;

public class ApiRoutes
{
    public const string BaseRoute = "api/v{version:apiVersion}/[Controller]";

    public class UserProfiles
    {
        public const string IdRoute = "{id}";
    }

    public class Post
    {
        public const string IdRoute = "{id}";
        public const string PostComments = "{postId}/comment";
        public const string CommentById = "{postId}/comment/{commentId}";
        public const string InteractionById = "{postId}/interactions/{interactionId}";
        public const string PostInteractions = "{postId}/interactions";
    }
    
    public static class Identity
    {
        public const string Login = "login";
        public const string Registration = "registration";
        public const string IdentityById = "identityUserId";
        public const string CurrentUser = "currentUser";
    }
    
}