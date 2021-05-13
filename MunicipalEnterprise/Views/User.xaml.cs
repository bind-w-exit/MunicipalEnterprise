using MunicipalEnterprise.ViewModels;
using System.Windows.Controls;

namespace MunicipalEnterprise.Views
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Page
    {
        public User()
        {
            InitializeComponent();
            DataContext = new UserViewModel();
            BaseViewModel.User = this;
        }
    }
}
