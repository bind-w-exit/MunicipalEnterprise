using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;
using MunicipalEnterprise.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MunicipalEnterprise.ViewModels
{
    class ComplaintsViewModel : BaseViewModel
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;
        private readonly IAuthService _authService;

        private ObservableCollection<Complaint> _complaintsList;
        public ObservableCollection<Complaint> ComplaintsList
        {
            get
            {
                return _complaintsList;
            }

            set { SetProperty(ref _complaintsList, value); }

        }

        private Complaint _selectedComplaint;
        public Complaint SelectedComplaint
        {
            get
            {
                return _selectedComplaint;
            }

            set { SetProperty(ref _selectedComplaint, value); }
        }

        private ObservableCollection<District> _districts;
        public ObservableCollection<District> Districts
        {
            get
            {
                return _districts;
            }

            set { SetProperty(ref _districts, value); }

        }

        private District _selectedDistrict;
        public District SelectedDistrict
        {
            get
            {
                return _selectedDistrict;
            }

            set
            {
                SetProperty(ref _selectedDistrict, value);
                ClearErrors(nameof(SelectedDistrict));
            }
        }

        private string _briefDescription;
        public string BriefDescription
        {
            get
            {
                return _briefDescription;
            }

            set
            {
                SetProperty(ref _briefDescription, value);
                ClearErrors(nameof(BriefDescription));
            }
        }

        public DelegateCommand DeleteComplainCommand { get; private set; }

        public ComplaintsViewModel(IDbContextFactory<MyDbContext> contextFactory, IAuthService authService)
        {
            _contextFactory = contextFactory;
            _authService = authService;

            OpenEditDialogCommand = new DelegateCommand(OpenEditDialog);
            OpenAddDialogCommand = new DelegateCommand(OpenAddDialog);
            AcceptDialogCommand = new DelegateCommand(AcceptDialog);
            CancelDialogCommand = new DelegateCommand(CancelDialog);
            DeleteComplainCommand = new DelegateCommand(DeleteComplain);

            using (var context = _contextFactory.CreateDbContext())
            {
                ComplaintsList = new ObservableCollection<Complaint>(context.Complaints.Where(x => x.User == _authService.User));
                Districts = new ObservableCollection<District>(context.Districts);
            }
        }       

        private void DeleteComplain(object obj)
        {                      
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Remove(SelectedComplaint);
                context.SaveChanges();
            }
            ComplaintsList.Remove(SelectedComplaint);
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
            BriefDescription = "";
            SelectedDistrict = Districts.FirstOrDefault();
            IsAddDialog = true;
            IsDialogOpen = true;          
        }

        private void OpenEditDialog(object obj)
        {
            if (SelectedComplaint != null)
            {
                DialogContent = new ComplaintsDialog();
                DialogTitle = "Editing complaint";
                BriefDescription = SelectedComplaint.Description;
                SelectedDistrict = Districts.FirstOrDefault();
                IsAddDialog = false;
                IsDialogOpen = true;               
            }
        }

        private void CancelDialog(object obj) => IsDialogOpen = false;

        private void AcceptDialog(object obj)
        {
            if (string.IsNullOrEmpty(BriefDescription))
            {
                AddError(nameof(BriefDescription), "Field cannot be empty");
            }
            if (SelectedDistrict == null)
            {
                AddError(nameof(SelectedDistrict), "Field cannot be empty");
            }
            if (!HasErrors)
            {

                using (var context = _contextFactory.CreateDbContext())
                {
                    var complain = new Complaint
                    {
                        Date = DateTime.Now,
                        Description = BriefDescription,
                        Status = "Not"
                    };

                    complain.User = context.Users.Find(_authService.User.Id);
                    complain.District = context.Districts.Find(SelectedDistrict.Id);

                    if (IsAddDialog)
                    {                       
                        context.Add(complain);
                        context.SaveChanges();
                        ComplaintsList.Add(complain);
                    }
                    else
                    {
                        context.Remove(SelectedComplaint);      //TODO FIX THIS!!!
                        context.Add(complain);
                        context.SaveChanges();
                        ComplaintsList.Remove(SelectedComplaint);
                        ComplaintsList.Add(complain);
                    }

                    IsDialogOpen = false;
                }              
            }                    
        }

        #endregion
    }
}
