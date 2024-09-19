using Microsoft.EntityFrameworkCore;
using SupPortal.NotificationService.API.Models;
using SupPortal.NotificationService.API.Repository.Abstract;

namespace SupPortal.NotificationService.API.Repository.Concrete;

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly nsContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(nsContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
  

    public async Task<List<T>> GetListAsync(
    Func<IQueryable<T>, IQueryable<T>> query = null)
    {
        query ??= q => q; 
        return await query(_dbSet).ToListAsync();
    }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

}
