using System.ComponentModel.DataAnnotations;

namespace Applications.DTO;

public class RegisterUserRequestDTO
{
    [EmailAddress(ErrorMessage = "Введите Email")]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}  