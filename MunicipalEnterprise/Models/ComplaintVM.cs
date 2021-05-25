using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Validators;
using System.Collections.ObjectModel;
using System.Linq;

namespace MunicipalEnterprise.Models
{
    public class ComplaintVM : ValidationBindableBase
    {
        private readonly ComplaintValidator validator;

        public ComplaintVM()
        {
            validator = new ComplaintValidator();
        }
        private ObservableCollection<Complaint> _complaintsList;
        public ObservableCollection<Complaint> ComplaintsList
        {
            get => _complaintsList;

            set
            {
                SetProperty(ref _complaintsList, value);
            }
        }

        private Complaint _selectedComplaint;
        public Complaint SelectedComplaint
        {
            get => _selectedComplaint;

            set
            {
                SetProperty(ref _selectedComplaint, value);
            }
        }

        private ObservableCollection<District> _districts;
        public ObservableCollection<District> Districts
        {
            get => _districts;

            set
            {
                SetProperty(ref _districts, value);
            }
        }

        private District _selectedDistrict;
        public District SelectedDistrict
        {
            get => _selectedDistrict;

            set
            {
                SetProperty(ref _selectedDistrict, value);
                ClearErrors(nameof(SelectedDistrict));
                var firstOrDefault = validator.Validate(this)
                    .Errors.FirstOrDefault(p => p.PropertyName == nameof(SelectedDistrict));
                if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                    AddError(nameof(SelectedDistrict), firstOrDefault.ErrorMessage);
            }
        }

        private string _briefDescription;
        public string BriefDescription
        {
            get => _briefDescription;

            set
            {
                SetProperty(ref _briefDescription, value);
                ClearErrors(nameof(BriefDescription));
                var firstOrDefault = validator.Validate(this)
                    .Errors.FirstOrDefault(p => p.PropertyName == nameof(BriefDescription));
                if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                    AddError(nameof(BriefDescription), firstOrDefault.ErrorMessage);
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
