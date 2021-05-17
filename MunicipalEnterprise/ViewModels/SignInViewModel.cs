using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using Prism.Commands;
using Prism.Regions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalEnterprise.ViewModels
{
    class SignInViewModel : BaseViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        private int _userId;

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                SetProperty(ref _login, value);
                ClearErrors(nameof(Login));
            }
        }

        public string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                ClearErrors(nameof(Password));
            }
        }

        public DelegateCommand<User> LoginCommand { get; private set; }

        public SignInViewModel(IRegionManager regionManager, IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _regionManager = regionManager;

            LoginCommand = new DelegateCommand<User>(LoginUser);
        }

        private async void LoginUser(User user)
        {
            await Task.Run(() =>
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Login == Login);
                    if (user != null)
                    {
                        if (user.Password == HashPassword(Password))
                        {
                            _userId = user.Id;
                        }
                        else
                        {
                            AddError(nameof(Password), "Wrong password");
                        }
                    }
                    else
                    {
                        AddError(nameof(Login), "User not found");
                    }
                }
            });

            if (!HasErrors)
            {
                var parameters = new NavigationParameters();
                parameters.Add("userId", _userId);
                _regionManager.RequestNavigate("MainRegion", "User", parameters);
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
