namespace SupPortal.UserService.API.Models.Dto;

public class ResetPasswordRequestDto
{
    public string Token { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
