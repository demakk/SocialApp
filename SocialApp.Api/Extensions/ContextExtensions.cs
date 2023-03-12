using System.Security.Claims;

namespace SocialApp.Extensions;

public static class ContextExtensions
{
    public static Guid GetUserProfileIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("UserProfileId", context);
    }

    public static Guid GetIdentityIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("IdentityId", context);
    }

    private static Guid GetGuidClaimValue(string key, HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        return Guid.Parse(identity?.FindFirst(key)?.Value);
    }
}