namespace SupPortal.NotificationService.API.Repository.Abstract;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<List<T>> GetListAsync(Func<IQueryable<T>, IQueryable<T>> query = null);
    Task<int> SaveChangesAsync();

}
