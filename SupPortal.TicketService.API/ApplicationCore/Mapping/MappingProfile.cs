using AutoMapper;
using SupPortal.Shared.Events;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.Domain.Entities;
using SupPortal.TicketService.API.Infrastructure.Extension;


namespace SupPortal.TicketService.API.ApplicationCore.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Ticket, GetTicketDto>();
        CreateMap<Ticket, CreateTicketEvent>();

        CreateMap<Tag, GetTagDto>();
        CreateMap<Comment, GetCommentDto>();

        CreateMap<PaginatedList<Ticket>, PaginatedResponseDto<GetTicketDto>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Items))
            .AfterMap((src, dest) =>
            {
                dest.PageNumber = src.PageNumber;
                dest.PageSize = src.PageSize;
                dest.TotalPages = src.TotalPages;
                dest.TotalCount = src.TotalCount;
            });


        CreateMap<PaginatedList<Tag>, PaginatedResponseDto<GetTagDto>>()
           .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Items))
           .AfterMap((src, dest) =>
           {
               dest.PageNumber = src.PageNumber;
               dest.PageSize = src.PageSize;
               dest.TotalPages = src.TotalPages;
               dest.TotalCount = src.TotalCount;
           });


        CreateMap<PaginatedList<Comment>, PaginatedResponseDto<GetCommentDto>>()
         .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Items))
         .AfterMap((src, dest) =>
         {
             dest.PageNumber = src.PageNumber;
             dest.PageSize = src.PageSize;
             dest.TotalPages = src.TotalPages;
             dest.TotalCount = src.TotalCount;
         });
    }
}

