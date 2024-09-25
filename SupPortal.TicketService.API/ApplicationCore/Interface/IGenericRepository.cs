using SupPortal.TicketService.API.Infrastructure.Extension;
using System.Linq.Expressions;

namespace SupPortal.TicketService.API.ApplicationCore.Interface;
public interface IGenericRepository<T> where T : class
{
    Task<PaginatedList<T>> ToPagedListAsync(QueryParameters? queryParameters = null, List<Expression<Func<T, object>>>? includes = null, CancellationToken? cancellationToken = null);

    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();

    Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken);
    Task<long> GetCount(CancellationToken cancellationToken);
}

