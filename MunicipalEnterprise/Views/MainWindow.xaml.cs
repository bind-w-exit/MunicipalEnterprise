using MunicipalEnterprise.Extensions;
using System.Windows;

namespace MunicipalEnterprise.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MigrationExtension.ApplyMigrations();
        }
    }
}
