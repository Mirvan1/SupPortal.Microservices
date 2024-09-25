using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Comment.Query;

public class GetAllCommentsQueryHandler(ICommentRepository _commentRepository,IMapper _mapper)
    : IRequestHandler<GetAllCommentsByTicketQuery, PaginatedResponseDto<GetCommentDto>>
{
    public async Task<PaginatedResponseDto<GetCommentDto>> Handle(GetAllCommentsByTicketQuery request, CancellationToken cancellationToken)
    {
        var getComments = await _commentRepository.GetCommentsByTicket(request.TicketId).ToPagedAsync(new QueryParameters(request.PageNumber, request.PageSize, request.SortBy, request.IsSortDescending));

        if (getComments is null) return PaginatedResponseDto<GetCommentDto>.Failure(ConstantErrorMessages.NotFound);

        var mappingRes = _mapper.Map<PaginatedResponseDto<GetCommentDto>>(getComments);

        if (mappingRes is null) return PaginatedResponseDto<GetCommentDto>.Failure(ConstantErrorMessages.NotFound);

        return mappingRes;
    }
}
