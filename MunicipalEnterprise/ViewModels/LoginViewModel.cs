using MunicipalEnterprise.Views;
using Prism.Commands;
using Prism.Regions;

namespace MunicipalEnterprise.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("LoginRegion", typeof(SignIn));
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("LoginRegion", navigatePath);
        }
    }
}
