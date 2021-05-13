using MunicipalEnterprise.ViewModels;
using System.Windows.Controls;

namespace MunicipalEnterprise.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}
