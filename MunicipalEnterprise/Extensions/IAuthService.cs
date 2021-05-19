using MunicipalEnterprise.Data.Models;

namespace MunicipalEnterprise.Extensions
{
    public interface IAuthService
    {
        User User { get; set; }
        bool Login(string login, string password);
        void Logout();
    }

}
