using FluentValidation;
using PhoneNumbers;
using webcore_backend.Features.Users.Dtos;

namespace webcore_backend.Features.Users.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequestDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(50).WithMessage("Username max length is 50 characters")
            .MinimumLength(3).WithMessage("Username minimum length is 3 characters")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Username must contain only alphanumeric characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(1).WithMessage("Password minimum length is 1 characters")
            .MaximumLength(50).WithMessage("Password maximum length is 50 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        
        /*RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Must(BeValidPhoneNumber).WithMessage("Phone number is invalid");*/
    }
    
    /*private bool BeValidPhoneNumber(string number)
    {
        var util = PhoneNumberUtil.GetInstance();
        try
        {
            var phone = util.Parse(number, null);
            return util.IsValidNumber(phone);
        }
        catch
        {
            return false;
        }
    }*/
}