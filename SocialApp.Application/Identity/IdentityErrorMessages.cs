namespace Social.Application.Identity;

public static class IdentityErrorMessages
{
    public const string NonExistentIdentityUser = "User with provided email does not exist.";
    public const string IncorrectPassword = "Provided password is incorrect";
    public const string IdentityUserAlreadyExists = "Provided user already exists. Cannot register new user";
    public const string UnauthorizedAccountRemoval = "You cannot remove account because you are not its owner";
}