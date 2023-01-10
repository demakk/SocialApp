﻿namespace Social.Domain.Aggregates.UserProfileAggregates;

public class UserProfile
{
    private UserProfile()
    {
        
    }
    public Guid Id { get; private set; }
    public string IdentityId { get; private set; }
    public BasicInfo BasicInfo { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime LastModified { get; private set; }

    //factory method
    public static UserProfile CreateUserProfile(string identityId, BasicInfo basicInfo)
    {
        //TO DO: add validation, error handling strategies, error notification
        
        return new UserProfile
        {
            IdentityId = identityId,
            BasicInfo = basicInfo,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }
    
    //public methods 

    public void UpdateBasicInfo(BasicInfo newInfo)
    {
        BasicInfo = newInfo;
        LastModified = DateTime.UtcNow;
    }
}