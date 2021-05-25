using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Extensions;
using System.Collections.ObjectModel;
using System.Linq;

namespace MunicipalEnterprise.ViewModels
{
    class HousesViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;
        private readonly IAuthService _authService;

        private ObservableCollection<House> _housesList;
        public ObservableCollection<House> HousesList { get => _housesList; set => SetProperty(ref _housesList, value); }

        public HousesViewModel(IDbContextFactory<MyDbContext> contextFactory, IAuthService authService)
        {
            _contextFactory = contextFactory;
            _authService = authService;

            using (var context = _contextFactory.CreateDbContext())
            {
                HousesList = new ObservableCollection<House>(context.Houses.Where(x => x.User == _authService.User));
            }
        }
    }
}
