using FluentValidation;
using MunicipalEnterprise.Models;
using System;
using System.Linq;

namespace MunicipalEnterprise.Validators
{
    class SignUpValidator : AbstractValidator<SignUpVM>
    {
        const string NullOrEmptyMessage = "Field cannot be empty";
        const string LettersOnlyMessage = "The field must contain only letters";

        public SignUpValidator()
        {
            CascadeMode = CascadeMode.Stop;     //Terminates the chain of inspections when a particular validator in the chain fails.

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
                .LessThan(DateTime.Today).WithMessage("Date of birth cannot be greater than {ComparisonValue}")
                .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Date of birth cannot be less than {ComparisonValue}");

            RuleFor(u => u.PhoneNum)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .Must(u => u.StartsWith("+")).WithMessage("Phone number must start with +")
                .Must(u => u.Substring(1).All(c => Char.IsDigit(c))).WithMessage("Phone number must contain only numbers")
                .Length(13).WithMessage("Length of the phone number must be 13 characters");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage(NullOrEmptyMessage)
                .EmailAddress().WithMessage("Іs not a valid email address");

            RuleFor(u => u.Login)
                .NotEmpty().WithMessage(NullOrEmptyMessage);

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage(NullOrEmptyMessage);

            //TODO
            //else
            //{
            //    var user = context.Users.FirstOrDefault(u => u.PhoneNum == PhoneNum);
            //    if (user != null)
            //    {
            //        AddError(nameof(PhoneNum), "Phone number is already exists");
            //    }
            //}
            //else
            //{
            //    var user = context.Users.FirstOrDefault(u => u.Email == Email);
            //    if (user != null)
            //    {
            //        AddError(nameof(Email), "Email already exists");
            //    }
            //}
            //else
            //{
            //    var user = context.Users.FirstOrDefault(u => u.Login == Login);
            //    if (user != null)
            //    {
            //        AddError(nameof(Login), "Login already exists");
            //    }
            //}
        }
    }
}
