using MunicipalEnterprise.Data.Models;
using Prism.Mvvm;
using System;

namespace MunicipalEnterprise.Models
{
    public class UserVM : BindableBase
    {
        public int Id { get; set; }

        private string _firstName;
        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }

        private string _lastName;
        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }

        private string _middleName;
        public string MiddleName { get => _middleName; set => SetProperty(ref _middleName, value); }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth { get => _dateOfBirth; set => SetProperty(ref _dateOfBirth, value); }

        private string _phoneNum;
        public string PhoneNum { get => _phoneNum; set => SetProperty(ref _phoneNum, value); }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _login;
        public string Login { get => _login; set => SetProperty(ref _login, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private Access _accessLevel;
        public Access AccessLevel { get => _accessLevel; set => SetProperty(ref _accessLevel, value); }
    }
}
