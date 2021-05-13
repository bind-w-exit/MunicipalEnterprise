using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using MunicipalEnterprise.Data;

namespace MunicipalEnterprise.ViewModels
{
    class SignInViewModel : BaseViewModel
    {
        private bool LoginFlag = false;

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                SetProperty(ref _login, value);

                ClearErrors(nameof(Login));
            }
        }

        public string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);

                ClearErrors(nameof(Password));
            }
        }

        public ICommand BtnClickLogin { get; private set; }

        public SignInViewModel()
        {
            BtnClickLogin = new DelegateCommand(BtnClickLoginCommand);
        }

        private async  void BtnClickLoginCommand(object obj)
        {
            var passwordBox = obj as PasswordBox;
            if (passwordBox == null)
                return;
            Password = passwordBox.Password;   
            
            await Task.Run(() =>
            {
                using (var context = new MyDbContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Login == Login);
                    if (user != null )
                    {
                        if (user.Password == HashPassword(Password))
                        {
                            LoginFlag = true;
                            UserId = user.Id;
                        }
                        else
                        {
                            AddError(nameof(Password), "Wrong password");
                        }
                    }
                    else
                    {
                        AddError(nameof(Login), "User not found");
                    }
                }
            });

            if (LoginFlag)
            {
                MainWindow.MainFrame.Navigate(new Views.User());
                LoginFlag = false;
            }        
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
