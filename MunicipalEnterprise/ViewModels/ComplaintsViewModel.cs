using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Extensions;
using MunicipalEnterprise.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MunicipalEnterprise.ViewModels
{
    class ComplaintsViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;
        private readonly IAuthService _authService;

        private ComplaintVM _complaint;
        public ComplaintVM Complaint { get => _complaint; set => SetProperty(ref _complaint, value); }

        public DelegateCommand DeleteComplainCommand { get; private set; }

        public ComplaintsViewModel(IDbContextFactory<MyDbContext> contextFactory, IAuthService authService)
        {
            _contextFactory = contextFactory;
            _authService = authService;

            _complaint = new();

            OpenEditDialogCommand = new DelegateCommand(OpenEditDialog);
            OpenAddDialogCommand = new DelegateCommand(OpenAddDialog);
            AcceptDialogCommand = new DelegateCommand(AcceptDialog);
            CancelDialogCommand = new DelegateCommand(CancelDialog);
            DeleteComplainCommand = new DelegateCommand(DeleteComplain);

            using (var context = _contextFactory.CreateDbContext())
            {
                Complaint.ComplaintsList = new ObservableCollection<Complaint>(context.Complaints.Where(x => x.User == _authService.User));
                Complaint.Districts = new ObservableCollection<District>(context.Districts);
            }
        }       

        private void DeleteComplain(object obj)
        {                      
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Remove(Complaint.SelectedComplaint);
                context.SaveChanges();
            }
            Complaint.ComplaintsList.Remove(Complaint.SelectedComplaint);
        }

        #region DialogWindow

        public DelegateCommand OpenAddDialogCommand { get; private set; }
        public DelegateCommand OpenEditDialogCommand { get; private set; }
        public DelegateCommand AcceptDialogCommand { get; private set; }
        public DelegateCommand CancelDialogCommand { get; private set; }

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get
            {
                return _isDialogOpen;
            }
            set { SetProperty(ref _isDialogOpen, value); }
        }

        private bool _isAddDialog;
        public bool IsAddDialog
        {
            get
            {
                return _isAddDialog;
            }
            set { SetProperty(ref _isAddDialog, value); }
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

        private string _dialogTitle;
        public string DialogTitle
        {
            get
            {
                return _dialogTitle;
            }
            set { SetProperty(ref _dialogTitle, value); }
        }

        private void OpenAddDialog(object obj)
        {
            DialogContent = new ComplaintsDialog();
            DialogTitle = "Add complaint";
            Complaint.BriefDescription = "";
            Complaint.SelectedDistrict = Complaint.Districts.FirstOrDefault();
            IsAddDialog = true;
            IsDialogOpen = true;          
        }

        private void OpenEditDialog(object obj)
        {
            if (Complaint.SelectedComplaint != null)
            {
                DialogContent = new ComplaintsDialog();
                DialogTitle = "Editing complaint";
                Complaint.BriefDescription = Complaint.SelectedComplaint.Description;
                Complaint.SelectedDistrict = Complaint.Districts.FirstOrDefault();
                IsAddDialog = false;
                IsDialogOpen = true;               
            }
        }

        private void CancelDialog(object obj) => IsDialogOpen = false;

        private void AcceptDialog(object obj)
        {
            if (Complaint.FullValidation())
            {

                using (var context = _contextFactory.CreateDbContext())
                {
                    var complain = new Complaint
                    {
                        Date = DateTime.Now,
                        Description = Complaint.BriefDescription,
                        Status = "Not"
                    };

                    complain.User = context.Users.Find(_authService.User.Id);
                    complain.District = context.Districts.Find(Complaint.SelectedDistrict.Id);

                    if (IsAddDialog)
                    {                       
                        context.Add(complain);
                        context.SaveChanges();
                        Complaint.ComplaintsList.Add(complain);
                    }
                    else
                    {
                        context.Update(Complaint.SelectedComplaint);      //TODO FIX THIS!!!
                        context.SaveChanges();
                        Complaint.ComplaintsList.Remove(Complaint.SelectedComplaint);
                        Complaint.ComplaintsList.Add(complain);
                    }

                    IsDialogOpen = false;
                }              
            }                    
        }

        #endregion
    }
}
