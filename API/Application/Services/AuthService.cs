using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Applications.DTO;
using Applications.DTO.Authorization.Responses;
using Azure.Core;
using FirstRestAPI.Common;
using FirstRestAPI.Common.Enums;
using Infrastructure.Models;
using Infrastructure.Models.InterfacesRepositories;
using Microsoft.IdentityModel.Tokens;


namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository userRepository;

    public AuthService(IUserRepository _userRepository)
    {
        userRepository = _userRepository;
    }

    public async Task<SuccessLoginResponseDTO> Register(RegisterUserRequestDTO reg_user)
    {
        var isRole = Enum.TryParse(reg_user.Role, true, out Roles _);
        if (!isRole)
        {
            throw new Exception("Неизвестная роль");
        }
        if (await userRepository.GetUserByEmail(reg_user.Email) is not null)
        {
            throw new Exception("Пользователь с данным email уже существует");
        }

        var hashedPassword = HashPassword.GetHashPassword(reg_user.PasswordHash);
        var user = await userRepository.AddUser(reg_user.Email, hashedPassword, reg_user.Role);
        return new SuccessLoginResponseDTO { RefreshToken = GenerateRefreshToken(user), AccessToken = GenerateAccessToken(user) };
    }


    public async Task<SuccessLoginResponseDTO> Login(LoginRequestDTO login_user)
    {
        var user = await userRepository.GetUserByEmail(login_user.Email);

        if (user is null)
        {
            throw new Exception("Неверный логин или пароль");
        }

        if (!HashPassword.VerifyHashedPassword(login_user.Password, user.PasswordHash))
        {
            throw new Exception("Неверный логин или пароль");
        }

        return new SuccessLoginResponseDTO { RefreshToken = GenerateRefreshToken(user), AccessToken = GenerateAccessToken(user) };
    }

    public async Task<SuccessLoginResponseDTO> RefreshToken(string refreshToken)
    {
        var user = await userRepository.GetUserById(refreshToken.GetUserId());
        if (user is null)
        {
            throw new Exception("Данного пользователя нет в системе");
        }
        return new SuccessLoginResponseDTO { RefreshToken = GenerateRefreshToken(user), AccessToken = GenerateAccessToken(user)};
    }
    private static string GenerateToken(List<Claim> claims, int timeExpireInMinute)
    {
        var now = DateTime.UtcNow;
     
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(timeExpireInMinute)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

    private static string GenerateAccessToken(User user) => GenerateToken(GetClaims(user), AuthOptions.ACCESS_TOKEN_LIFETIME);

    private static string GenerateRefreshToken(User user) =>
        GenerateToken(GetClaims(user), AuthOptions.REFRESH_TOKEN_LIFETIME);

    private static List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimType.Id.ToString(), user.UserId.ToString()),
            new (ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
        };
        return claims;
    }
}