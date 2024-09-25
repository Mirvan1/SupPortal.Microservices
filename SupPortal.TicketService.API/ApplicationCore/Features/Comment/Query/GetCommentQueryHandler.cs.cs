using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Comment.Query;

public class GetCommentQueryHandler(ICommentRepository _commentRepository,IMapper _mapper) : IRequestHandler<GetCommentQuery, GetCommentDto>
{
    public async Task<GetCommentDto> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        var getComment = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (getComment is null) return BaseResponseDto.ErrorResponse<GetCommentDto>(ConstantErrorMessages.NotFound);

        var mappingRes = _mapper.Map<GetCommentDto>(getComment);

        if (mappingRes is null) return BaseResponseDto.ErrorResponse<GetCommentDto>(ConstantErrorMessages.NotFound);

        return mappingRes;
    }
}