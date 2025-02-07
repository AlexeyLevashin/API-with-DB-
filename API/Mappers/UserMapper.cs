using FirstRestAPI.Domain;
using Infrastructure.Models;
using FirstRestAPI.Mappers;

namespace FirstRestAPI.Mappers;

public static class UserMapper
{
    public static User MapToDb(this DomUser user)
    {
        return new User { Email = user.Email, Role = user.Role, UserId = user.UserId };
    }

    public static User? MapToDomain(this DomUser? User)
    {
        return User is null ? null : new User { Email = User.Email, Role = User.Role, UserId = User.UserId };
    }
}