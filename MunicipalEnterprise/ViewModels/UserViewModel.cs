using MunicipalEnterprise.Views;
using Prism.Commands;
using Prism.Regions;

namespace MunicipalEnterprise.ViewModels
{
    class UserViewModel : BaseViewModel
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public UserViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("UserRegion", typeof(Houses));
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("UserRegion", navigatePath);
        }
    }
}
