using MunicipalEnterprise.ViewModels;
using System.Windows.Controls;

namespace MunicipalEnterprise.Views
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Page
    {
        public SignIn()
        {
            InitializeComponent();
            DataContext = new SignInViewModel();
        }
    }
}
