using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Tag.Command;

public class CreateTagCommandHandler(ITagRepository _tagRepository,IMapper mapper,IAuthSettings _authSettings)
         : IRequestHandler<CreateTagCommand, BaseResponseDto>
{
    public async Task<BaseResponseDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var checkTagExist = await _tagRepository.GetByName(request.TagName,cancellationToken);

        if (checkTagExist is not null) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.BadRequest);

        var newTag = new Domain.Entities.Tag()
        {
            Name = request.TagName,
            UserName = _authSettings.GetLoggedUsername(),
        };

        await _tagRepository.AddAsync(newTag);
        int res =await  _tagRepository.SaveChangesAsync(cancellationToken);

        return BaseResponseDto.Response(res > 0);

    }
}
