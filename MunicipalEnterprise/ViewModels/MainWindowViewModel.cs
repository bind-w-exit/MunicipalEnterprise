using MunicipalEnterprise.Data;
using System.Windows.Controls;

namespace MunicipalEnterprise.ViewModels
{ 
    public class MainWindowViewModel : BaseViewModel
    {        
        public Page Login;

        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage == value)
                    return;

                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public MainWindowViewModel()
        {
            using (var context = new MyDbContext())
            {
                context.SaveChanges();
            }

            Login = new Views.Login();
            CurrentPage = Login;
        }      
    }
}
