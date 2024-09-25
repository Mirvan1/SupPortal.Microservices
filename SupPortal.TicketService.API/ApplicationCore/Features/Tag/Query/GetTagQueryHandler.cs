using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Infrastructure.Extension;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Tag.Query;

public class GetTagQueryHandler(ITagRepository _tagRepository, IMapper _mapper) : IRequestHandler<GetTagQuery, GetTagDto>
{
    public async Task<GetTagDto> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        var getTag = await _tagRepository.GetByName(request.TagName, cancellationToken);

        if (getTag is null) return BaseResponseDto.ErrorResponse<GetTagDto>(ConstantErrorMessages.NotFound);

        var mappingRes = _mapper.Map<GetTagDto>(getTag);

        if (mappingRes is null) return BaseResponseDto.ErrorResponse<GetTagDto>(ConstantErrorMessages.NotFound);

        mappingRes.isSuccess = true;

        return mappingRes;
    }
}
