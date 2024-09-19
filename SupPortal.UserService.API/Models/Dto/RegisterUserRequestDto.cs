using SupPortal.UserService.API.Models.Entities;

namespace SupPortal.UserService.API.Models.Dto;

public class RegisterUserRequestDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public DateTime BirthDate{ get; set; }
}
