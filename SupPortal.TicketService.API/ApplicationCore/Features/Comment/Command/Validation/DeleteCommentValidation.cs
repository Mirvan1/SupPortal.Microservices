using FluentValidation;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;

namespace SupPortal.TicketService.API.ApplicationCore.Features.Comment.Command.Validation;

public class DeleteCommentValidation:AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentValidation()
    {
        RuleFor(x => x.CommentId)
   .NotNull().WithMessage("Cannot be blank");
    }
}
