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
    }
}