using MunicipalEnterprise.Validators;
using System.Linq;

namespace MunicipalEnterprise.Models
{
    public class SignInVM : ValidationBindableBase
    {
        private readonly SignInValidator validator;

        public SignInVM()
        {
            validator = new();
        }

        private string _login;
        public string Login
        {
            get => _login;

            set
            {
                if(SetProperty(ref _login, value))
                {
                    ClearErrors(nameof(Login));
                    var firstOrDefault = validator.Validate(this)
                        .Errors.FirstOrDefault(p => p.PropertyName == nameof(Login));
                    if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                        AddError(nameof(Login), firstOrDefault.ErrorMessage);
                }
            }
        }

        public string _password;
        public string Password
        {
            get => _password; 

            set
            {
                if(SetProperty(ref _password, value))
                {
                    ClearErrors(nameof(Password));
                    var firstOrDefault = validator.Validate(this)
                        .Errors.FirstOrDefault(p => p.PropertyName == nameof(Password));
                    if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                        AddError(nameof(Password), firstOrDefault.ErrorMessage);
                }
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

        public void WrongLoginOrPassword()
        {
            AddError(nameof(Login), "Wrong login or password");
        }
    }
}
