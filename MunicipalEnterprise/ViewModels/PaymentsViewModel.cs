using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Views;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MunicipalEnterprise.ViewModels
{
    public class PaymentsViewModel : BaseViewModel, INavigationAware
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        private int _userId;

        private ObservableCollection<Payment> _paymentsList;
        public ObservableCollection<Payment> PaymentsList
        {
            get
            {
                return _paymentsList;
            }

            set { SetProperty(ref _paymentsList, value); }

        }

        private Complaint _selectedPayment;
        public Complaint SelectedPayment
        {
            get
            {
                return _selectedPayment;
            }

            set { SetProperty(ref _selectedPayment, value); }
        }

        private ObservableCollection<House> _housesList;
        public ObservableCollection<House> HousesList
        {
            get
            {
                return _housesList;
            }

            set { SetProperty(ref _housesList, value); }

        }

        private House _selectedHouse;
        public House SelectedHouse
        {
            get
            {
                return _selectedHouse;
            }

            set
            {
                OldHeatMeter = Convert.ToString(SelectedHouse?.HeatMeter);
                SetProperty(ref _selectedHouse, value);
            }
        }

        private string _cost;
        public string Cost
        {
            get
            {
                return _cost;
            }

            set
            {
                SetProperty(ref _cost, value);

                ClearErrors(nameof(Cost));

                if (value.Length > 0)
                    if (!DataValidation.Numeric(value))
                        AddError(nameof(Cost), "Invalid number format");
            }
        }

        private string _oldHeatMeter;
        public string OldHeatMeter
        {
            get
            {
                return _oldHeatMeter;
            }

            set { SetProperty(ref _oldHeatMeter, value); }
        }

        private string _heatMeter;
        public string HeatMeter
        {
            get
            {
                return _heatMeter;
            }

            set
            {
                SetProperty(ref _heatMeter, value);

                ClearErrors(nameof(HeatMeter));

                if (value.Length > 0)
                    if (!DataValidation.Numeric(value))
                        AddError(nameof(HeatMeter), "Invalid number format");
                    else
                        if (Convert.ToInt32(HeatMeter) < Convert.ToInt32(OldHeatMeter))
                        AddError(nameof(HeatMeter), "The indicator can be less than the old rate");
            }
        }

        public PaymentsViewModel(IDbContextFactory<MyDbContext> contextFactory)
        {
            OpenDialogCommand = new DelegateCommand(OpenDialog);
            AcceptDialogCommand = new DelegateCommand(AcceptDialog);
            CancelDialogCommand = new DelegateCommand(CancelDialog);

            _contextFactory = contextFactory;
        }

        #region DialogWindow

        public ICommand OpenDialogCommand { get; }
        public ICommand AcceptDialogCommand { get; }
        public ICommand CancelDialogCommand { get; }

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get
            {
                return _isDialogOpen;
            }
            set { SetProperty(ref _isDialogOpen, value); }
        }

        private object _dialogContent;
        public object DialogContent
        {
            get
            {
                return _dialogContent;
            }
            set { SetProperty(ref _dialogContent, value); }
        }

        private void OpenDialog(object obj)
        {
            DialogContent = new PaymentsDialog();
            Cost = "";
            HeatMeter = "";
            
            IsDialogOpen = true;
        }


        private void CancelDialog(object obj) => IsDialogOpen = false;

        private void AcceptDialog(object obj)
        {
            
            if (SelectedHouse == null)
            {
                AddError(nameof(SelectedHouse), "Field cannot be empty");
            }
            if (string.IsNullOrEmpty(HeatMeter))
            {
                AddError(nameof(HeatMeter), "Field cannot be empty");
            }
            if (string.IsNullOrEmpty(Cost))
            {
                AddError(nameof(Cost), "Field cannot be empty");
            }

            if (!HasErrors)
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var payment = new Payment
                    {
                        Date = DateTime.Now,
                        Cost = Convert.ToInt32(Cost),
                        HeatMeter = Convert.ToInt32(HeatMeter)
                    };

                    var user = context.Users.FirstOrDefault(u => u.Id == _userId);
                    var house = context.Houses.FirstOrDefault(u => u.Id == SelectedHouse.Id);

                    house.HeatMeter = Convert.ToInt32(HeatMeter);

                    payment.User = user;
                    payment.House = house;
                    PaymentsList.Add(payment);
                    context.SaveChanges();
                    SelectedHouse = null;
                    IsDialogOpen = false;
                }
            }
        }

        #endregion

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var userId = navigationContext.Parameters["userId"];
            if (userId != null)
                _userId = (int)userId;

            using (var context = _contextFactory.CreateDbContext())
            {
                context.Payments.Where(x => x.User.Id == _userId).Load();
                context.Houses.Where(x => x.User.Id == _userId).Load();
                PaymentsList = context.Payments.Local.ToObservableCollection();
                HousesList = context.Houses.Local.ToObservableCollection();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
