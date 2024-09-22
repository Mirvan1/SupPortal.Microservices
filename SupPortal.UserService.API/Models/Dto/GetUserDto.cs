namespace SupPortal.UserService.API.Models.Dto;

public class GetUserDto : BaseResponse
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool? IsActive { get; set; }

    public GetUserDto(string? username, string? email, string? firstName, string? lastName, bool? isActive)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive;
    }

}
