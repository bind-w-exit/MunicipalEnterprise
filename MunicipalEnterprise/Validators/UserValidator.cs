using FluentValidation;
using MunicipalEnterprise.Models;
using System;
using System.Linq;

namespace MunicipalEnterprise.Validators
{
    class UserValidator : AbstractValidator<UserVM>
    {
        const string NullOrEmptyMessage = "Field cannot be empty";
        const string LettersOnlyMessage = "The field must contain only letters";

        public UserValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .Must(u => u.All(Char.IsLetter)).WithMessage(LettersOnlyMessage);

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .Must(u => u.All(Char.IsLetter)).WithMessage(LettersOnlyMessage);

            RuleFor(u => u.MiddleName)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .Must(u => u.All(Char.IsLetter)).WithMessage(LettersOnlyMessage);

            RuleFor(u => u.DateOfBirth)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .LessThan(DateTime.Today.AddYears(-120)).WithMessage("Date of birth cannot be less than {ComparisonValue}")
                .GreaterThan(DateTime.Today).WithMessage("Date of birth cannot be greater than {ComparisonValue}");

            RuleFor(u => u.PhoneNum)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .Must(u => u.StartsWith("+")).WithMessage("Phone number must start with +")
                .Must(u => u.Substring(1).All(c => Char.IsDigit(c))).WithMessage("Phone number must contain only numbers")
                .Length(12).WithMessage("Length of the phone number must be 13 characters");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .EmailAddress().WithMessage("Іs not a valid email address");

            RuleFor(u => u.Login)
                .NotEmpty().WithMessage(NullOrEmptyMessage);

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage(NullOrEmptyMessage);

            RuleFor(u => u.AccessLevel)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .IsInEnum().WithMessage("Access level was entered incorrectly");
        }
    }
}
