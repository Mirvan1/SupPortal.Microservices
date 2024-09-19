namespace SupPortal.UserService.API.Models.Entities;

public class Role
{
    public int Id { get; set; } 
    public RoleName Name { get; set; }
    public string Description { get; set; }

    public ICollection<User> Users { get; set; }

}


public enum RoleName
{
    Admin=1,
    Supporter=2,
    User=3
}