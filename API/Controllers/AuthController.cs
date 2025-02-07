using Application.Interfaces;
using Applications.DTO;
using Microsoft.AspNetCore.Mvc;
namespace FirstRestAPI.Controllers;

[ApiController]
[Route("api/auth/")]
public class AuthController:ControllerBase
{
    private IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }
    
    [HttpPost ("register")]
    public async Task<IActionResult> Register(RegisterUserRequestDTO regUser)
    {
        return Ok(await authService.Register(regUser));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO logUser)
    {
        return Ok(await authService.Login(logUser));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        return Ok(await authService.RefreshToken(refreshToken));
    }
}