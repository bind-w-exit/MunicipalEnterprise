using MunicipalEnterprise.Extensions;
using MunicipalEnterprise.Models;
using Prism.Regions;
using System.Threading.Tasks;

namespace MunicipalEnterprise.ViewModels
{
    class SignInViewModel : BaseViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IAuthService _authService;

        private SignInVM _signIn;
        public SignInVM SignIn { get => _signIn; set => SetProperty(ref _signIn, value); }

        public DelegateCommand LoginCommand { get; private set; }

        public SignInViewModel(IRegionManager regionManager, IAuthService authService)
        {
            _regionManager = regionManager;
            _authService = authService;

            _signIn = new();

            LoginCommand = new DelegateCommand(LoginUser);
        }

        private void LoginUser(object obj)
        {
            if(SignIn.FullValidation())
            {
                if (_authService.Login(SignIn.Login, SignIn.Password))
                    _regionManager.RequestNavigate("MainRegion", "User");
                else SignIn.WrongLoginOrPassword(); //TODO Add error "Wrong login or password"
            }
        }
    }
}
