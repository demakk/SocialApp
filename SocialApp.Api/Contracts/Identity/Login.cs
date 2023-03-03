using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.Contracts.Identity;

public class Login
{
    [EmailAddress]
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}