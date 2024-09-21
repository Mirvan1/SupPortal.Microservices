using FluentValidation;
using SupPortal.UserService.API.Models.Dto;

namespace SupPortal.UserService.API.Data.Service.Validations;

public class RegisterUserValidator:AbstractValidator<RegisterUserRequestDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Cannot be blank")
      .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Cannot be blank")
    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
    .Matches("[0-9]").WithMessage("Password must contain at least one number.")
    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");


        RuleFor(x => x.FirstName)
          .NotEmpty()
          .WithMessage("Cannot be blank")
          .MinimumLength(3)
          .WithMessage("Cannot be blank")
          .MaximumLength(100)
          .WithMessage("Cannot be blank");


        RuleFor(x => x.LastName)
          .NotEmpty()
          .WithMessage("Cannot be blank")
          .MinimumLength(3)
          .WithMessage("Cannot be blank")
          .MaximumLength(100)
          .WithMessage("Cannot be blank");


        RuleFor(x => x.UserName)
          .NotEmpty()
          .WithMessage("Cannot be blank")
          .MinimumLength(3)
          .WithMessage("Cannot be blank")
          .MaximumLength(100)
          .WithMessage("Cannot be blank");
    }
}
