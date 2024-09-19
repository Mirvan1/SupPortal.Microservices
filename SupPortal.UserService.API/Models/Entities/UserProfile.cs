namespace SupPortal.UserService.API.Models.Entities;

    public class UserProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public DateTime DateOfBirth { get; set; }  
        public DateTime CreatedAt { get; set; }  
        public DateTime LastLogin { get; set; }

    public User User { get; set; }
}
