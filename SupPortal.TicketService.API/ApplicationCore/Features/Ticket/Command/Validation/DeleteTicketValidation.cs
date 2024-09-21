using FluentValidation;
using MediatR;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Command.Validation;

public class DeleteTicketValidation:AbstractValidator<DeleteTicketCommand>
{
    public DeleteTicketValidation()
    {
        RuleFor(x => x.TicketId)
    .NotNull().WithMessage("Cannot be blank");
    }
}
