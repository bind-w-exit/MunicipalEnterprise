using FluentValidation;
using MunicipalEnterprise.Models;

namespace MunicipalEnterprise.Validators
{
    public class SignInValidator : AbstractValidator<SignInVM>
    {
        const string NullOrEmptyMessage = "Field cannot be empty";

        public SignInValidator()
        {
            RuleFor(u => u.Login)
                .NotEmpty().WithMessage(NullOrEmptyMessage);

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage(NullOrEmptyMessage);
        }
    }
}
