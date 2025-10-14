using FluentValidation;
using webcore_backend.Features.Auth.Dtos;

namespace webcore_backend.Features.Auth.Validators;

public class AuthUserRequestValidator : AbstractValidator<AuthUserRequestDto>
{
    public AuthUserRequestValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Email) || !string.IsNullOrWhiteSpace(x.Username))
            .WithMessage("Either Email or Username must be provided.");

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.");
        });
    }
}