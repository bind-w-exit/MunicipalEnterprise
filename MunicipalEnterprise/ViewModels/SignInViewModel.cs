using MunicipalEnterprise.Extensions;
using Prism.Regions;
using System.Threading.Tasks;

namespace MunicipalEnterprise.ViewModels
{
    class SignInViewModel : BaseViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IAuthService _authService;

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
                ClearErrors(nameof(Login));
            }
        }

        public DelegateCommand LoginCommand { get; private set; }

        public SignInViewModel(IRegionManager regionManager, IAuthService authService)
        {
            _regionManager = regionManager;
            _authService = authService;

            LoginCommand = new DelegateCommand(LoginUser);
        }

        private async void LoginUser(object obj)
        {
            await Task.Run(() =>
            {
                if (!_authService.Login(Login, Password))
                    AddError(nameof(Login), "Wrong login or password");
                
            });

            if (!HasErrors)
            {
                _regionManager.RequestNavigate("MainRegion", "User");
            }
        }
    }
}
