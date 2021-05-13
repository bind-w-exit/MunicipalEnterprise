using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using System.Windows.Controls;

namespace MunicipalEnterprise.ViewModels
{ 
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        public Page Login;

        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }

        public MainWindowViewModel(IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            using (var context = _contextFactory.CreateDbContext())
            {
                context.SaveChanges();
            }

            Login = new Views.Login();
            CurrentPage = Login;
        }      
    }
}
