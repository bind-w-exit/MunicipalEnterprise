using Prism.Commands;
using Prism.Regions;

namespace MunicipalEnterprise.ViewModels
{
    class UserViewModel : BaseViewModel, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        private int _userId;

        private NavigationParameters parameters;

        public DelegateCommand<string> NavigateCommand { get; private set; }


        public UserViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;         
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("UserRegion", navigatePath, parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var userId = navigationContext.Parameters["userId"];
            if (userId != null)
                _userId = (int)userId;

            parameters = new NavigationParameters();
            parameters.Add("userId", _userId);
            _regionManager.RequestNavigate("UserRegion", "Houses", parameters);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
