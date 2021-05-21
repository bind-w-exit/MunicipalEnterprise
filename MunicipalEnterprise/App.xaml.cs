using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Extensions;
using MunicipalEnterprise.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;

namespace MunicipalEnterprise
{
    public partial class App : PrismApplication
    {
        /// <summary>
        /// This is the method that will create the main window of the application. 
        /// </summary>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();     //The Container property of the App class should be used to create the window as it takes care of any dependencies.
        }

        /// <summary>
        /// This function is used to register any app dependencies.
        /// </summary>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.Models.User, Models.UserVM>();
                cfg.CreateMap<Models.UserVM, Data.Models.User>();
            });

            containerRegistry.RegisterInstance<IMapper>(mapperConfiguration.CreateMapper());

            containerRegistry.RegisterSingleton<IDbContextFactory<MyDbContext>, MyDbContextFactory>();
            containerRegistry.RegisterSingleton<IAuthService, AuthManager>();

            containerRegistry.RegisterForNavigation<SignIn>();
            containerRegistry.RegisterForNavigation<SignUp>();
            containerRegistry.RegisterForNavigation<User>();
            containerRegistry.RegisterForNavigation<Houses>();
            containerRegistry.RegisterForNavigation<Payments>();
            containerRegistry.RegisterForNavigation<Complaints>();
            containerRegistry.RegisterForNavigation<UserAccount>();
        }

    }
}
