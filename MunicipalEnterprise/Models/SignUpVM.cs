using MunicipalEnterprise.Validators;
using System;
using System.Linq;

namespace MunicipalEnterprise.Models
{
    public class SignUpVM : ValidationBindableBase
    {
        private readonly SignUpValidator validator;

        public SignUpVM()
        {
            validator = new();
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;

            set
            {
                if(SetProperty(ref _firstName, value))
                {
                    ClearErrors(nameof(FirstName));
                    var firstOrDefault = validator.Validate(this)
                        .Errors.FirstOrDefault(p => p.PropertyName == nameof(FirstName));
                    if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                        AddError(nameof(FirstName), firstOrDefault.ErrorMessage);
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;

            set
            {
                if(SetProperty(ref _lastName, value))
                {
                    ClearErrors(nameof(LastName));
                    var firstOrDefault = validator.Validate(this)
                        .Errors.FirstOrDefault(p => p.PropertyName == nameof(LastName));
                    if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                        AddError(nameof(LastName), firstOrDefault.ErrorMessage);
                }
            }
        }

        private string _middleName;
        public string MiddleName
        {
            get => _middleName;

            set
            {
                if(SetProperty(ref _middleName, value))
                {
                    ClearErrors(nameof(MiddleName));
                    var firstOrDefault = validator.Validate(this)
                        .Errors.FirstOrDefault(p => p.PropertyName == nameof(MiddleName));
                    if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                        AddError(nameof(MiddleName), firstOrDefault.ErrorMessage);
                }
            }
        }

        private DateTime _dateOfBirth = DateTime.Today;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;

            set
            {
                if(SetProperty(ref _dateOfBirth, value))
                {
                    ClearErrors(nameof(DateOfBirth));
                    var firstOrDefault = validator.Validate(this)
                        .Errors.FirstOrDefault(p => p.PropertyName == nameof(DateOfBirth));
                    if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                        AddError(nameof(DateOfBirth), firstOrDefault.ErrorMessage);
                }
            }
        }

        private string _phoneNum;
        public string PhoneNum
        {
            get => _phoneNum;

            set
            {
                if(SetProperty(ref _phoneNum, value))
                {
                    ClearErrors(nameof(PhoneNum));
                    var firstOrDefault = validator.Validate(this)
                        .Errors.FirstOrDefault(p => p.PropertyName == nameof(PhoneNum));
                    if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                        AddError(nameof(PhoneNum), firstOrDefault.ErrorMessage);
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;

            set
            {
                if(SetProperty(ref _email, value))
                {
                    ClearErrors(nameof(Email));
                    var firstOrDefault = validator.Validate(this)
                        .Errors.FirstOrDefault(p => p.PropertyName == nameof(Email));
                    if (firstOrDefault != null && firstOrDefault.ErrorCode != "NotEmptyValidator")
                        AddError(nameof(Email), firstOrDefault.ErrorMessage);
                }
            }
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

        private string _password;
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
    }
}
