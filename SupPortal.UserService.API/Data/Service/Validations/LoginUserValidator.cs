using FluentValidation;
using SupPortal.UserService.API.Models.Dto;

namespace SupPortal.UserService.API.Data.Service.Validations;

public class LoginUserValidator:AbstractValidator<LoginUserRequestDto>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Username)
           .NotNull()
           .WithMessage("Cannot be blank")
           .MinimumLength(3)
           .WithMessage("Cannot be blank")
           .MaximumLength(100)
           .WithMessage("Cannot be blank");

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100)
            .WithMessage("Cannot be blank");
    }
}
