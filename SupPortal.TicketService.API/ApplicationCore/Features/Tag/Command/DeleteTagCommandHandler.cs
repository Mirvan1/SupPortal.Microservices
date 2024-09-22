using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Response;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.Infrastructure.Repository;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Tag.Command;

public class DeleteTagCommandHandler(IAuthSettings _authSettings,ITagRepository _tagRepository) : IRequestHandler<DeleteTagCommand, BaseResponseDto>
{
    public async Task<BaseResponseDto> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var loggedUserRole = _authSettings.GetLoggedUserRole();
        var loggedUserName = _authSettings.GetLoggedUsername();

        var getTag = await _tagRepository.GetByIdAsync(request.TagId);

        if (getTag is null) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.NotFound);

        if (!getTag.UserName.Equals(loggedUserName) || loggedUserRole.Equals("Supporter")) return BaseResponseDto.ErrorResponse(ConstantErrorMessages.UnAuthorized);

        await _tagRepository.DeleteAsync(getTag);
        await _tagRepository.SaveChangesAsync();

        return BaseResponseDto.SuccessResponse();
    }
}
