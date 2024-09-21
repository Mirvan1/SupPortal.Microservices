using FluentValidation;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Command.Validation;

public class UpdateTicketValidation:AbstractValidator<UpdateTicketStatusCommand>
{
    public UpdateTicketValidation()
    {
        RuleFor(x => x.TicketId)
       .NotNull().WithMessage("Cannot be blank");

        RuleFor(x => x.TicketStatus)
            .NotNull().WithMessage("Cannot be blank");

    }
}
