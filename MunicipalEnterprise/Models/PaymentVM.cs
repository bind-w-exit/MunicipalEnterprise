using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Validators;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MunicipalEnterprise.Models
{
    public class PaymentVM : ValidationBindableBase
    {
        private readonly PaymentValidator validator;

        public PaymentVM()
        {
            validator = new PaymentValidator();
        }

        private ObservableCollection<Payment> _payments;
        public ObservableCollection<Payment> Payments { get => _payments; set => SetProperty(ref _payments, value); }

        private ObservableCollection<House> _houses;
        public ObservableCollection<House> Houses { get => _houses; set => SetProperty(ref _houses, value); }

        private House _selectedHouse;
        public House SelectedHouse
        {
            get => _selectedHouse;

            set
            {
                SetProperty(ref _selectedHouse, value);
                OldHeatMeter = SelectedHouse.HeatMeter;
                ClearErrors(nameof(SelectedHouse));
                var firstOrDefault = validator.Validate(this)
                    .Errors.FirstOrDefault(p => p.PropertyName == nameof(SelectedHouse));
                if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                    AddError(nameof(SelectedHouse), firstOrDefault.ErrorMessage);
            }
        }

        private int _oldHeatMeter;
        public int OldHeatMeter { get => _oldHeatMeter; set => SetProperty(ref _oldHeatMeter, value); }

        private int _heatMeter;
        public int HeatMeter
        {
            get => _heatMeter;

            set
            {
                SetProperty(ref _heatMeter, value);
                ClearErrors(nameof(HeatMeter));
                var firstOrDefault = validator.Validate(this)
                    .Errors.FirstOrDefault(p => p.PropertyName == nameof(HeatMeter));
                if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                    AddError(nameof(HeatMeter), firstOrDefault.ErrorMessage);
            }
        }

        private int _cost;
        public int Cost
        {
            get => _cost;

            set
            {
                SetProperty(ref _cost, value);
                ClearErrors(nameof(Cost));
                var firstOrDefault = validator.Validate(this)
                    .Errors.FirstOrDefault(p => p.PropertyName == nameof(Cost));
                if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                    AddError(nameof(Cost), firstOrDefault.ErrorMessage);
            }
        }

        public bool FullValidation()
        {
            var validationResult = validator.Validate(this);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    AddError(failure.PropertyName, failure.ErrorMessage);
                }
                return false;
            }
            else return true;
        }
    }
}
