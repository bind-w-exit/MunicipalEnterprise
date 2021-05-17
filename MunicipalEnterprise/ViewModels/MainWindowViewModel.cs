using MunicipalEnterprise.Views;
using Prism.Ioc;
using Prism.Regions;

namespace MunicipalEnterprise.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IContainerExtension container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(Login));
        }      
    }
}
