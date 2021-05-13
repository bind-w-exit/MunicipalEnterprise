using System.Windows.Controls;
using System.Windows.Input;


namespace MunicipalEnterprise.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private Page SignIn;
        private Page SignUp;
     
        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }

        public ICommand BtnClickSignIn
        {
            get;
            private set;
        }

        public ICommand BtnClickSignUp
        {
            get;
            private set;
        }

        public LoginViewModel()
        {

            SignIn = new Views.SignIn();
            SignUp = new Views.SignUp();

            BtnClickSignIn = new DelegateCommand(BtnClickSignInCommand);
            BtnClickSignUp = new DelegateCommand(BtnClickSignUpCommand);

            CurrentPage = SignIn;
        }

        private void BtnClickSignUpCommand(object obj)
        {
            CurrentPage = SignUp;
        }

        private void BtnClickSignInCommand(object obj)
        {
            CurrentPage = SignIn;
        }
    }
}
