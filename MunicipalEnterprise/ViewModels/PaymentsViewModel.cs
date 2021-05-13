using MunicipalEnterprise.Views;
using System;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Input;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Data;

namespace MunicipalEnterprise.ViewModels
{
    class PaymentsViewModel : BaseViewModel
    {  
       MyDbContext context = new MyDbContext();

        private ObservableCollection<Payment> _paymentsList;
        public ObservableCollection<Payment> PaymentsList
        {
            get
            {
                return _paymentsList;
            }

            set
            {
                _paymentsList = value;
                OnPropertyChanged(nameof(PaymentsList));
            }

        }

        private Complaint _selectedPayment;
        public Complaint SelectedPayment
        {
            get
            {
                return _selectedPayment;
            }

            set
            {
                _selectedPayment = value;
                OnPropertyChanged(nameof(SelectedPayment));
            }
        }

        private ObservableCollection<House> _housesList;
        public ObservableCollection<House> HousesList
        {
            get
            {
                return _housesList;
            }

            set
            {
                _housesList = value;
                OnPropertyChanged(nameof(HousesList));
            }

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
                _selectedHouse = value;
                OldHeatMeter = Convert.ToString(SelectedHouse?.HeatMeter);
                OnPropertyChanged(nameof(SelectedHouse));
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
                _cost = value;
                OnPropertyChanged(nameof(Cost));

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

            set
            {
                _oldHeatMeter = value;
                OnPropertyChanged(nameof(OldHeatMeter));
            }
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
                _heatMeter = value;
                OnPropertyChanged(nameof(HeatMeter));

                ClearErrors(nameof(HeatMeter));

                if (value.Length > 0)
                    if (!DataValidation.Numeric(value))
                        AddError(nameof(HeatMeter), "Invalid number format");
                    else
                        if (Convert.ToInt32(HeatMeter) < Convert.ToInt32(OldHeatMeter))
                        AddError(nameof(HeatMeter), "The indicator can be less than the old rate");
            }
        }

        public PaymentsViewModel()
        {

            OpenDialogCommand = new DelegateCommand(OpenDialog);
            AcceptDialogCommand = new DelegateCommand(AcceptDialog);
            CancelDialogCommand = new DelegateCommand(CancelDialog);


            context.Payments.Where(x => x.User.Id == UserId).Load();
            context.Houses.Where(x => x.User.Id == UserId).Load();
            PaymentsList = context.Payments.Local.ToObservableCollection();
            HousesList = context.Houses.Local.ToObservableCollection();
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
            set
            {
                _isDialogOpen = value;
                OnPropertyChanged(nameof(IsDialogOpen));
            }
        }

        private object _dialogContent;
        public object DialogContent
        {
            get
            {
                return _dialogContent;
            }
            set
            {
                _dialogContent = value;
                OnPropertyChanged(nameof(DialogContent));
            }
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
                var payment = new Payment
                {
                    Date = DateTime.Now,
                    Cost = Convert.ToInt32(Cost),
                    HeatMeter = Convert.ToInt32(HeatMeter)
                };

                var user = context.Users.FirstOrDefault(u => u.Id == UserId);
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

        #endregion
    }
}
