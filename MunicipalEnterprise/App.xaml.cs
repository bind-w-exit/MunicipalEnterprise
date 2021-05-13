using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Views;
using Prism.Ioc;

namespace MunicipalEnterprise
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDbContextFactory<MyDbContext>, MyDbContextFactory>();
        }

    }
}
