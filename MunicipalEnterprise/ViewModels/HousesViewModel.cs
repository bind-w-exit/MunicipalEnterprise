using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace MunicipalEnterprise.ViewModels
{
    class HousesViewModel : BaseViewModel, INavigationAware
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        private int _userId;

        private ObservableCollection<House> _housesList;
        public ObservableCollection<House> HousesList
        {
            get
            {
                return _housesList;
            }

            set { SetProperty(ref _housesList, value); }

        }

        public HousesViewModel(IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var userId = navigationContext.Parameters["userId"];
            if (userId != null)
                _userId = (int)userId;

            using (var context = _contextFactory.CreateDbContext())
            {
                context.Houses.Where(x => x.User.Id == _userId).Load();
                HousesList = context.Houses.Local.ToObservableCollection();
            }
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
