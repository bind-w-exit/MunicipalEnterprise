using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MunicipalEnterprise.ViewModels
{
    class UserAccountViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        public ICommand BtnClickSaveChanges
        {
            get;
            private set;
        }

        public ICommand BtnClickUndoChanges
        {
            get;
            private set;
        }

        public UserAccountViewModel(IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            BtnClickSaveChanges = new DelegateCommand(BtnClickSaveChangesCommand);
            BtnClickUndoChanges = new DelegateCommand(BtnClickUndoChangesCommand);

            using (var context = _contextFactory.CreateDbContext())
            {
                var user = context.Users.Find(UserId);
                if (user != null)
                {
                    FirstName = user.FirstName;
                    LastName = user.LastName;
                    MiddleName = user.MiddleName;
                    DateOfBirth = user.DateOfBirth;
                    PhoneNum = user.PhoneNum;
                    Email = user.Email;
                    Login = user.Login;

                    _backupFirstName = user.FirstName;
                    _backupLastName = user.LastName;
                    _backupMiddleName = user.MiddleName;
                    _backupDateOfBirth = user.DateOfBirth;
                    _backupPhoneNum = user.PhoneNum;
                    _backupEmail = user.Email;
                    _backupLogin = user.Login;
                    _backupPassword = user.Password;
                }
            }
        }

        private string _backupFirstName; //TODO User
        private string _backupLastName;
        private string _backupMiddleName;
        private DateTime _backupDateOfBirth;
        private string _backupPhoneNum;
        private string _backupEmail;
        private string _backupLogin;
        private string _backupPassword;

        private string _firstName = "";
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

        private string _lastName = "";
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

        private string _middleName = "";
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

        private DateTime _dateOfBirth = DateTime.Today;
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

        private string _phoneNum = "";
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

        private string _email = "";
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

        private string _login = "";
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

        public string _password = "";

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                    return;

                SetProperty(ref _password, value);

                ClearErrors(nameof(Password));
            }
        }

        private async void BtnClickSaveChangesCommand(object obj)
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
                        if (user != null &&  context.Users.Find(UserId).PhoneNum != PhoneNum)
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
                        if (user != null && context.Users.Find(UserId).Email != Email)
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
                        if (user != null && context.Users.Find(UserId).Login != Login)
                        {
                            AddError(nameof(Login), "Login already exists");
                        }
                    }
                    if (Password == "")
                    {
                        AddError(nameof(Password), "Field cannot be empty");
                    }

                    if (!HasErrors)
                    {
                        var user = context.Users.Find(UserId);

                        user.FirstName = FirstName;
                        user.LastName = LastName;
                        user.MiddleName = MiddleName;
                        user.DateOfBirth = DateOfBirth;
                        user.PhoneNum = PhoneNum;
                        user.Email = Email;
                        user.Login = Login;
                        if(!String.IsNullOrEmpty(Password))
                            user.Password = HashPassword(Password);

                        context.SaveChanges();

                    }
                }
            });

        }

        private void BtnClickUndoChangesCommand(object obj)
        {
            FirstName = _backupFirstName;
            LastName = _backupLastName;
            MiddleName = _backupMiddleName;
            DateOfBirth = _backupDateOfBirth;
            PhoneNum = _backupPhoneNum;
            Email = _backupEmail;
            Login = _backupLogin;
            Password = _backupPassword;

            using (var context = _contextFactory.CreateDbContext())
            {
                var user = context.Users.Find(UserId);

                user.FirstName = _backupFirstName;
                user.LastName = _backupLastName;
                user.MiddleName = _backupMiddleName;
                user.DateOfBirth = _backupDateOfBirth;
                user.PhoneNum = _backupPhoneNum;
                user.Email = _backupEmail;
                user.Login = _backupLogin;
                user.Password = _backupPassword;

                context.SaveChanges();
            }
        }

        public string HashPassword(string s)
        { 
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
    }
}
