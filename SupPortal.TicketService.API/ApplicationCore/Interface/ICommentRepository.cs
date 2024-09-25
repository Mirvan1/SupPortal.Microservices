using SupPortal.TicketService.API.Domain.Entities;

namespace SupPortal.TicketService.API.ApplicationCore.Interface;
public interface ICommentRepository : IGenericRepository<Comment>
{
    IQueryable<Comment> GetCommentsByTicket(int RequestId);
}

