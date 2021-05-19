using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MunicipalEnterprise.ViewModels
{
    class UserAccountViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;
        private readonly IAuthService _authService;

        public DelegateCommand SaveChangesCommand { get; private set; }
        public DelegateCommand UndoChangesCommand { get; private set; }

        public UserAccountViewModel(IDbContextFactory<MyDbContext> contextFactory, IAuthService authService)
        {
            _contextFactory = contextFactory;
            _authService = authService;

            SaveChangesCommand = new DelegateCommand(SaveChanges);
            UndoChangesCommand = new DelegateCommand(UndoChanges);

            using (var context = _contextFactory.CreateDbContext())
            {
                var user = context.Users.Find(_authService.User.Id);
                if (user != null)
                {
                    FirstName = user.FirstName;
                    LastName = user.LastName;
                    MiddleName = user.MiddleName;
                    DateOfBirth = user.DateOfBirth;
                    PhoneNum = user.PhoneNum;
                    Email = user.Email;
                    Login = user.Login;
                    AccessLevel = user.AccessLevel.ToString();

                    _backupUser = user;
                }
            }
        }

        private User _backupUser;

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                    return;

                SetProperty(ref _firstName, value);

                ClearErrors(nameof(FirstName));

                if (!value.All(x => x >= 'a' && x <= 'z' || x >= 'A' && x <= 'Z'))
                {
                    AddError(nameof(FirstName), "Invalid name format");
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value)
                    return;

                SetProperty(ref _lastName, value);

                ClearErrors(nameof(LastName));

                if (!value.All(x => x >= 'a' && x <= 'z' || x >= 'A' && x <= 'Z'))
                {
                    AddError(nameof(LastName), "Invalid name format");
                }
            }
        }

        private string _middleName;
        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                if (_middleName == value)
                    return;

                SetProperty(ref _middleName, value);

                ClearErrors(nameof(MiddleName));

                if (!value.All(x => x >= 'a' && x <= 'z' || x >= 'A' && x <= 'Z'))
                {
                    AddError(nameof(MiddleName), "Invalid name format");
                }
            }
        }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (_dateOfBirth == value)
                    return;

                SetProperty(ref _dateOfBirth, value);

                ClearErrors(nameof(DateOfBirth));

                if (value.Year > DateTime.Now.Year || value.Year <= 1920)
                {
                    AddError(nameof(DateOfBirth), "Invalid date format");
                }
            }
        }

        private string _phoneNum;
        public string PhoneNum
        {
            get { return _phoneNum; }
            set
            {
                if (_phoneNum == value)
                    return;

                SetProperty(ref _phoneNum, value);

                ClearErrors(nameof(PhoneNum));

                if (value.Length > 0)
                {
                    if (value.Length != 13)
                    {
                        AddError(nameof(PhoneNum), "Invalid number format");
                    }
                    if (!value.All(x => x >= '0' && x <= '9' || x == '+'))
                    {
                        AddError(nameof(PhoneNum), "Invalid number format");
                    }

                    if (!(value.FirstOrDefault() == '+'))
                    {
                        AddError(nameof(PhoneNum), "Invalid number format");
                    }
                }
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value)
                    return;

                SetProperty(ref _email, value);

                ClearErrors(nameof(Email));

                if (value.Length > 0)
                {
                    if (!value.Any(x => x == '@'))
                    {
                        AddError(nameof(Email), "Invalid email format");
                    }

                    if (!value.Any(x => x == '.'))
                    {
                        AddError(nameof(Email), "Invalid email format");
                    }
                }

            }
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                if (_login == value)
                    return;

                SetProperty(ref _login, value);

                ClearErrors(nameof(Login));
            }
        }

        private string _accessLevel;       
        public string AccessLevel
        {
            get { return _accessLevel; }
            set
            {
                if (_accessLevel == value)
                    return;

                SetProperty(ref _accessLevel, value);

                ClearErrors(nameof(AccessLevel));
            }
        }

        private async void SaveChanges(object obj)
        {
            await Task.Run(() =>
            {
                using (var context = _contextFactory.CreateDbContext())
                {

                    if (FirstName == "")
                    {
                        AddError(nameof(FirstName), "Field cannot be empty");
                    }
                    if (LastName == "")
                    {
                        AddError(nameof(LastName), "Field cannot be empty");
                    }
                    if (MiddleName == "")
                    {
                        AddError(nameof(MiddleName), "Field cannot be empty");
                    }
                    if (DateOfBirth.Year >= DateTime.Now.Year || DateOfBirth.Year <= 1920)
                    {
                        AddError(nameof(DateOfBirth), "Field cannot be empty");
                    }
                    if (PhoneNum == "")
                    {
                        AddError(nameof(PhoneNum), "Field cannot be empty");
                    }
                    else
                    {
                        var user = context.Users.FirstOrDefault(u => u.PhoneNum == PhoneNum);
                        if (user != null &&  context.Users.Find(_authService.User.Id).PhoneNum != PhoneNum)
                        {
                            AddError(nameof(PhoneNum), "Phone number is already exists");
                        }
                    }
                    if (Email == "")
                    {
                        AddError(nameof(Email), "Field cannot be empty");
                    }
                    else
                    {
                        var user = context.Users.FirstOrDefault(u => u.Email == Email);
                        if (user != null && context.Users.Find(_authService.User.Id).Email != Email)
                        {
                            AddError(nameof(Email), "Email already exists");
                        }
                    }
                    if (Login == "")
                    {
                        AddError(nameof(Login), "Field cannot be empty");
                    }
                    else
                    {
                        var user = context.Users.FirstOrDefault(u => u.Login == Login);
                        if (user != null && context.Users.Find(_authService.User.Id).Login != Login)
                        {
                            AddError(nameof(Login), "Login already exists");
                        }
                    }

                    if (!HasErrors)
                    {
                        var user = context.Users.Find(_authService.User.Id);

                        user.FirstName = FirstName;
                        user.LastName = LastName;
                        user.MiddleName = MiddleName;
                        user.DateOfBirth = DateOfBirth;
                        user.PhoneNum = PhoneNum;
                        user.Email = Email;
                        user.Login = Login;

                        context.SaveChanges();

                    }
                }
            });

        }

        private async void UndoChanges(object obj)
        {
            FirstName = _backupUser.FirstName;
            LastName = _backupUser.LastName;
            MiddleName = _backupUser.MiddleName;
            DateOfBirth = _backupUser.DateOfBirth;
            PhoneNum = _backupUser.PhoneNum;
            Email = _backupUser.Email;
            Login = _backupUser.Login;

            await Task.Run(() =>
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var user = context.Users.Find(_authService.User.Id);

                    user = _backupUser;
                    context.SaveChanges();
                }
            });
        }

    }
}
