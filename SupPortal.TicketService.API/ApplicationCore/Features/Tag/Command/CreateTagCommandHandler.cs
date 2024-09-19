using AutoMapper;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Tag.Command;

public class CreateTagCommandHandler(ITagRepository _tagRepository,IMapper mapper)
         : IRequestHandler<CreateTagCommand, BaseResponseDto>
{
    public async Task<BaseResponseDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {

        var newTag = new Domain.Entities.Tag()
        {
            Name = request.TagName
        };
        await _tagRepository.AddAsync(newTag);
        int res =await  _tagRepository.SaveChangesAsync();

        return BaseResponseDto.Response(res > 0);

    }
}
