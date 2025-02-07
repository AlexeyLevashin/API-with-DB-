using FirstRestAPI.Common.Enums;

namespace Infrastructure.Models;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Roles Role { get; set; }
}