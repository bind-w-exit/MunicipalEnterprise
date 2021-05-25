using FluentValidation;
using MunicipalEnterprise.Models;

namespace MunicipalEnterprise.Validators
{
    public class PaymentValidator : AbstractValidator<PaymentVM>
    {
        const string NullOrEmptyMessage = "Field cannot be empty";
        //const string NumbersOnlyMessage = "The field should contain only numbers";

        public PaymentValidator()
        {
            CascadeMode = CascadeMode.Stop;     //Terminates the chain of inspections when a particular validator in the chain fails.

            RuleFor(u => u.SelectedHouse)
                .NotEmpty().WithMessage(NullOrEmptyMessage);

            RuleFor(u => u.HeatMeter)
                .NotEmpty().WithMessage(NullOrEmptyMessage)             
                .GreaterThan(u => u.OldHeatMeter).WithMessage("Heat meter cannot be less than {ComparisonValue}");

            RuleFor(u => u.Cost)
                .NotEmpty().WithMessage(NullOrEmptyMessage);
        }
    }
}
