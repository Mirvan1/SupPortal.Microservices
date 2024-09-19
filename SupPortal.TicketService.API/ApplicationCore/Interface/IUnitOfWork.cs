namespace SupPortal.TicketService.API.ApplicationCore.Interface;

public interface IUnitOfWork 
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
