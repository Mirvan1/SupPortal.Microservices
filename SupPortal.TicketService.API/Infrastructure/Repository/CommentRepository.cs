using Microsoft.EntityFrameworkCore;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Domain.Entities;
using SupPortal.TicketService.API.Infrastructure.Data;

namespace SupPortal.TicketService.API.Infrastructure.Repository;
public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(tsContext context) : base(context)
    {
    }


    public IQueryable<Comment> GetCommentsByTicket(int RequestId)
    {
        return _dbSet.Where(x=>x.TicketId == RequestId);
    }
}
