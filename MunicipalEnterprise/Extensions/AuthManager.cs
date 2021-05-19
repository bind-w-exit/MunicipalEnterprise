using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MunicipalEnterprise.Extensions
{
    public class AuthManager : IAuthService
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        public User User { get; set; }

        public AuthManager(IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public bool Login(string login, string password)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login);
                if (user != null)
                {
                    if (user.Password == HashPassword(password))
                    {
                        User = user;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public void Logout()
        {
            User = null;
        }

        public string HashPassword(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
    }
}
