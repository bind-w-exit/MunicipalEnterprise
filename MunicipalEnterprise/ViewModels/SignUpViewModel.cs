using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Models;
using System.Threading.Tasks;
using System.Windows.Input;


namespace MunicipalEnterprise.ViewModels
{
    class SignUpViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public ICommand RegisterCommand { get; private set; }

        private SignUpVM _signUp;
        public SignUpVM SignUp { get => _signUp; set => SetProperty(ref _signUp, value); }

        public SignUpViewModel(IDbContextFactory<MyDbContext> contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;

            _signUp = new();

            RegisterCommand = new DelegateCommand(Register);
        }     

        private async void Register(object obj)     //TODO Register success
        {
            if(SignUp.FullValidation())
                await Task.Run(() =>
                {
                    using (var context = _contextFactory.CreateDbContext())
                    {
                        var user = _mapper.Map<SignUpVM, Data.Models.User>(SignUp); //TODO Hash password

                        context.Users.Add(user);
                        context.SaveChanges();

                        SignUp = new();
                    }
                });
        }
    }
}
