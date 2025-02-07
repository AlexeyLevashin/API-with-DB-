namespace FirstRestAPI.Common.Enums;

using System.Text;
using Microsoft.IdentityModel.Tokens;

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer";
    public const string AUDIENCE = "MyAuthClient";
    private const string KEY = "mysupersecrwrqwrqwret_secretkey!123";
    public const int ACCESS_TOKEN_LIFETIME = 60 * 2;
    public const int REFRESH_TOKEN_LIFETIME = 60 * 24 * 7;

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}