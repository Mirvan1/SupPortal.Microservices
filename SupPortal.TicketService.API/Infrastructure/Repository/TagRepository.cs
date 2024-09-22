using Microsoft.EntityFrameworkCore;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Domain.Entities;
using SupPortal.TicketService.API.Infrastructure.Data;

namespace SupPortal.TicketService.API.Infrastructure.Repository;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(tsContext context) : base(context)
    {
    }

    public async Task<Tag> GetByName(string name)
    {
      return await _dbSet.FirstOrDefaultAsync(x => x.Name.Equals(name));
    }
}

