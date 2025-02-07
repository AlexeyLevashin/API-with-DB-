using System.ComponentModel.DataAnnotations;
namespace Applications.DTO;

public class LoginRequestDTO
{
    [EmailAddress(ErrorMessage = "Некорректный email адрес")]
    public string Email { get; set; }
    public string Password { get; set; }
}