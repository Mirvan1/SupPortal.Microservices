using FluentValidation;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Comment.Command.Validation;

public class CreateCommenValidation : AbstractValidator<CreateCommentCommand>
{
    public CreateCommenValidation()
    {
        RuleFor(x => x.TicketId)
        .NotNull().WithMessage("Cannot be blank");

        RuleFor(x => x.CommentContent)
      .NotEmpty().WithMessage("Cannot be blank")
      .MaximumLength(100).WithMessage("Cannot be blank")
      .MinimumLength(3).WithMessage("Cannot be blank");
    }
}
