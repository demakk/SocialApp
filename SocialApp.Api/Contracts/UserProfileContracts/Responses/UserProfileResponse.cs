namespace SocialApp.Contracts.UserProfileContracts.Responses;

public record UserProfileResponse
{
    public Guid Id { get;  set; }
    public BasicInfoResponse BasicInfo { get;  set; }
    public DateTime DateCreated { get;  set; }
    public DateTime LastModified { get;  set; }
}