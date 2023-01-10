﻿using MediatR;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Application.UserProfiles.Commands;

public class CreateUserCommand : IRequest<UserProfile>
{
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public string EmailAddress { get;  set; }
    public string Phone { get;  set; }
    public DateTime DateOfBirth { get;  set; }
    public string CurrentCity { get;  set; }
}