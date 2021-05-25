using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Extensions;
using MunicipalEnterprise.Models;
using MunicipalEnterprise.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MunicipalEnterprise.ViewModels
{
    public class PaymentsViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        private PaymentVM _payments;
        public PaymentVM Payments { get => _payments; set => SetProperty(ref _payments, value); }

        public PaymentsViewModel(IDbContextFactory<MyDbContext> contextFactory, IAuthService authService)
        {
            OpenDialogCommand = new DelegateCommand(OpenDialog);
            AcceptDialogCommand = new DelegateCommand(AcceptDialog);
            CancelDialogCommand = new DelegateCommand(CancelDialog);

            _authService = authService;
            _contextFactory = contextFactory;

            _payments = new();

            using (var context = _contextFactory.CreateDbContext())
            {
                Payments.Payments = new ObservableCollection<Payment>(context.Payments.Where(x => x.User == _authService.User));
                Payments.Houses = new ObservableCollection<House>(context.Houses.Where(x => x.User == _authService.User));
            }
        }

        #region DialogWindow

        public ICommand OpenDialogCommand { get; }
        public ICommand AcceptDialogCommand { get; }
        public ICommand CancelDialogCommand { get; }

        private readonly IAuthService _authService;
        private bool _isDialogOpen;
        public bool IsDialogOpen { get => _isDialogOpen; set => SetProperty(ref _isDialogOpen, value); }

        private object _dialogContent;
        public object DialogContent { get => _dialogContent; set => SetProperty(ref _dialogContent, value); }

        private void OpenDialog(object obj)
        {
            DialogContent = new PaymentsDialog();       //FIX THIS NAVIGATE
            Payments.SelectedHouse = Payments.Houses.FirstOrDefault();
            Payments.HeatMeter = 0;
            Payments.Cost = 0;

            IsDialogOpen = true;
        }


        private void CancelDialog(object obj) => IsDialogOpen = false;

        private void AcceptDialog(object obj)
        {           
            if (Payments.FullValidation())
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var payment = new Payment
                    {
                        Date = DateTime.Now,
                        Cost = Payments.Cost,
                        HeatMeter = Payments.HeatMeter
                    };

                    Payments.SelectedHouse.HeatMeter = Payments.HeatMeter;

                    payment.User = _authService.User;
                    payment.House = Payments.SelectedHouse;
                    Payments.Payments.Add(payment);
                    context.Update(Payments.SelectedHouse);
                    context.Update(_authService.User);      //FIX OR NOT?
                    context.Add(payment);
                    context.SaveChanges();               
                    IsDialogOpen = false;
                }
            }
        }

        #endregion

    }
}
