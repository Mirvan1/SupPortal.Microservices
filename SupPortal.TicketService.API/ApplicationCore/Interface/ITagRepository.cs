using SupPortal.TicketService.API.Domain.Entities;

namespace SupPortal.TicketService.API.ApplicationCore.Interface;
public interface ITagRepository : IGenericRepository<Tag>
{
    Task<Tag> GetByName(string name);
}

