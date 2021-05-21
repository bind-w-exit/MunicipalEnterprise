using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Extensions;
using MunicipalEnterprise.Models;
using MunicipalEnterprise.Validators;
using System.Threading.Tasks;

namespace MunicipalEnterprise.ViewModels
{
    class UserAccountViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IValidator<UserVM> _validator;

        private UserVM _backupUser;
        private UserVM _user;
        public UserVM User { get => _user; set => SetProperty(ref _user, value); }

        public DelegateCommand SaveChangesCommand { get; private set; }
        public DelegateCommand UndoChangesCommand { get; private set; }


        public UserAccountViewModel(IDbContextFactory<MyDbContext> contextFactory, IAuthService authService, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _authService = authService;
            _mapper = mapper;
            _validator = new UserValidator();

            User = _mapper.Map<Data.Models.User, UserVM>(_authService.User);
            _backupUser = _mapper.Map<Data.Models.User, UserVM>(_authService.User);

            SaveChangesCommand = new DelegateCommand(SaveChanges);
            UndoChangesCommand = new DelegateCommand(UndoChanges);

        }     

        private async void SaveChanges(object obj)
        {

             
            var validationResult = _validator.Validate(User);
            if (validationResult.IsValid)
            {
                _authService.User = _mapper.Map<UserVM, Data.Models.User>(User);
                await Task.Run(() =>
                {
                    using (var context = _contextFactory.CreateDbContext())
                    {
                        var user = _mapper.Map<UserVM, Data.Models.User>(User);
                        context.Update(user);
                        context.SaveChanges();
                    }
                });
            }
            else
            {
                foreach (var failure in validationResult.Errors)
                {
                    AddError(failure.PropertyName, failure.ErrorMessage);
                }
            }


        }

        private async void UndoChanges(object obj)
        {
            _authService.User = _mapper.Map<UserVM, Data.Models.User>(_backupUser);
            User = _mapper.Map<Data.Models.User, UserVM>(_authService.User);
            await Task.Run(() =>
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var user = _mapper.Map<UserVM, Data.Models.User>(_backupUser);
                    context.Update(user);
                    context.SaveChanges();
                }
            });     
        }
    }
}
