using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Data;

namespace MunicipalEnterprise.ViewModels
{
    class HousesViewModel : BaseViewModel
    {
        private ObservableCollection<House> _housesList;
        public ObservableCollection<House> HousesList
        {
            get
            {
                return _housesList;
            }

            set { SetProperty(ref _housesList, value); }

        }

        public HousesViewModel()
        {
            using (var context = new MyDbContext())
            {
                context.Houses.Where(x => x.User.Id == UserId).Load();
                HousesList = context.Houses.Local.ToObservableCollection(); ;
            }
        }
    }
}
