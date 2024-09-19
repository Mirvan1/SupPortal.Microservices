using SupPortal.UserService.API.Models.Entities;

namespace SupPortal.UserService.API.Data.Repository.Abstract;

public interface IUserRepository  
{

    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
    Task AddAsync(User entity);
    void Update(User entity);
    void Delete(User entity);
    Task<int> SaveChangesAsync();

    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserByEmailAsync(string email);
    Task<Role> GetStandUserRole();


}
