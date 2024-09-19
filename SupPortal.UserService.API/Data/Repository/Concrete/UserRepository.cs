using Microsoft.EntityFrameworkCore;
using SupPortal.UserService.API.Data.Context;
using SupPortal.UserService.API.Data.Repository.Abstract;
using SupPortal.UserService.API.Models.Entities;
using System;

namespace SupPortal.UserService.API.Data.Repository.Concrete;

 
    public class UserRepository: IUserRepository 
    {
        private readonly userDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(userDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(User entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(User entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(User entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

      public async Task<Role> GetStandUserRole()
    {
        return await _context.Roles.Where(x => x.Name == RoleName.User).FirstOrDefaultAsync();
    }
}

