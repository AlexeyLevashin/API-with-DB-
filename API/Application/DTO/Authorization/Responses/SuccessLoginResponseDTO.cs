namespace Applications.DTO.Authorization.Responses;

public class SuccessLoginResponseDTO
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}