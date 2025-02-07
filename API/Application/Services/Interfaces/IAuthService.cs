using Applications.DTO;
using Applications.DTO.Authorization.Responses;

namespace Application.Interfaces;

public interface IAuthService
{
    public Task<SuccessLoginResponseDTO> Register(RegisterUserRequestDTO reg_user);
    public Task<SuccessLoginResponseDTO> Login(LoginRequestDTO login_user);
    public Task<SuccessLoginResponseDTO> RefreshToken(string refresh);

}