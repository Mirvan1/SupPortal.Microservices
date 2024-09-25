using Microsoft.EntityFrameworkCore;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Infrastructure.Data;
using SupPortal.TicketService.API.Infrastructure.Extension;
using System.Linq.Expressions;

namespace SupPortal.TicketService.API.Infrastructure.Repository;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly tsContext _context;
    internal readonly DbSet<T> _dbSet;

    public GenericRepository(tsContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id,CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id,cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<PaginatedList<T>> ToPagedListAsync(QueryParameters? param = null, List<Expression<Func<T, object>>>? includes = null,CancellationToken? cancellationToken=null)
    {
        if (param is not null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!string.IsNullOrEmpty(param?.SortBy))
            {
                query = query.CustomOrderByProperty(param.SortBy, (bool)param.IsSortDescending);

            }

            if (!string.IsNullOrEmpty(param?.SearchText) && !string.IsNullOrEmpty(param.SearchBy))
            {
                query = query.SearchByPropertyContains(param.SearchBy, param.SearchText);
            }

            var count = await query.CountAsync();

            List<T> items = null;
            if (cancellationToken==null)
            {
                items = await query.Skip((param.PageNumber) * param.PageSize)
                                  .Take(param.PageSize)
                                  .ToListAsync();
            }
            else
            {
                  items = await query.Skip((param.PageNumber) * param.PageSize)
                                 .Take(param.PageSize)
                                 .ToListAsync((CancellationToken)cancellationToken);
            }
          

            return new PaginatedList<T>(items, count, param.PageNumber, param.PageSize == int.MaxValue ? count : param.PageSize);
        }
        else
        {
            return new PaginatedList<T>(await _context.Set<T>().ToListAsync(), await _context.Set<T>().CountAsync(), 0, await _context.Set<T>().CountAsync());
        }
    }


    public async Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(cancellationToken);
    }


    public async Task<long> GetCount(CancellationToken cancellationToken)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }
}
