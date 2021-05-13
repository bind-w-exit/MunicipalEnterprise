using MunicipalEnterprise.Extensions;
using MunicipalEnterprise.ViewModels;
using System.Windows;

namespace MunicipalEnterprise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            MigrationExtension.ApplyMigrations();
            BaseViewModel.MainWindow = this;
        }
    }
}
