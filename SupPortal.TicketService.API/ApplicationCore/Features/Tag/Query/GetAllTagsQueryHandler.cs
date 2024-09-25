using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Tag.Query;
public class GetAllTagsQueryHandler(ITagRepository _tagRepository,IMapper _mapper) : IRequestHandler<GetAllTagsQuery, PaginatedResponseDto<GetTagDto>>
{
    public async Task<PaginatedResponseDto<GetTagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var getTags = await _tagRepository.ToPagedListAsync(new QueryParameters(request.PageNumber, request.PageSize, request.SortBy, request.IsSortDescending),null,cancellationToken);

        if (getTags is null) return PaginatedResponseDto<GetTagDto>.Failure(ConstantErrorMessages.NotFound);

        var mappingRes = _mapper.Map<PaginatedResponseDto<GetTagDto>>(getTags);

        if (mappingRes is null) return PaginatedResponseDto<GetTagDto>.Failure(ConstantErrorMessages.NotFound);

        return mappingRes;
    }
}

