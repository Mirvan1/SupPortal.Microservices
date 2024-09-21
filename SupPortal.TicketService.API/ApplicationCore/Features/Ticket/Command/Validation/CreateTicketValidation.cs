using FluentValidation;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Ticket.Command.Validation;

public class CreateTicketValidation:AbstractValidator<CreateTicketCommand>
{
    public CreateTicketValidation()
    {
        RuleFor(x => x.Name)
       .NotEmpty().WithMessage("Cannot be blank")
       .MaximumLength(100).WithMessage("Cannot be blank")
       .MinimumLength(3).WithMessage("Cannot be blank");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Cannot be blank")
            .MinimumLength(3).WithMessage("Cannot be blank");


        RuleFor(x => x.Status)
       .NotNull().WithMessage("Cannot be blank");

        RuleFor(x => x.Priority)
            .NotNull().WithMessage("Cannot be blank");

        RuleFor(x => x.TagId)
        .NotNull().WithMessage("Cannot be blank");

    }
}
