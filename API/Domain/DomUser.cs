using FirstRestAPI.Common.Enums;

namespace FirstRestAPI.Domain;

public class DomUser
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Roles Role { get; set; }
}