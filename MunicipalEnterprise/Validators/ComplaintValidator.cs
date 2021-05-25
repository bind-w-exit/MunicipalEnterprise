using FluentValidation;
using MunicipalEnterprise.Models;

namespace MunicipalEnterprise.Validators
{
    public class ComplaintValidator : AbstractValidator<ComplaintVM>
    {
        const string NullOrEmptyMessage = "Field cannot be empty";

        public ComplaintValidator()
        {
            RuleFor(u => u.SelectedDistrict)
                .NotEmpty().WithMessage(NullOrEmptyMessage);

            RuleFor(u => u.BriefDescription)
                .NotEmpty().WithMessage(NullOrEmptyMessage);
        }
    }
}
