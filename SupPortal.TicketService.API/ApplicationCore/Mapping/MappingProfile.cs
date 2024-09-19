using AutoMapper;
using SupPortal.Shared.Events;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Domain.Entities;


namespace SupPortal.TicketService.API.ApplicationCore.Mapping;
public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<Ticket, GetTicketDto>();
        CreateMap<Ticket, CreateTicketEvent>();

        CreateMap<Tag, GetTagDto>();
        CreateMap<GetCommentDto, GetCommentDto>();
    }
}

