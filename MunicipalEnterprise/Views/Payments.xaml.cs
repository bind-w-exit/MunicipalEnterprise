using MunicipalEnterprise.ViewModels;
using System.Windows.Controls;

namespace MunicipalEnterprise.Views
{
    /// <summary>
    /// Interaction logic for Payments.xaml
    /// </summary>
    public partial class Payments : Page
    {
        public Payments()
        {
            InitializeComponent();
            DataContext = new PaymentsViewModel();
        }
    }
}
