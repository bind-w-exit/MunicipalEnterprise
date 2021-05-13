using System;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Input;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Models;

namespace MunicipalEnterprise.ViewModels
{
    class ComplaintsViewModel : BaseViewModel
    {
        MyDbContext context = new MyDbContext();

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

        public ComplaintsViewModel()
        {
            //Dialog window
            OpenEditDialogCommand = new DelegateCommand(OpenEditDialog);
            OpenAddDialogCommand = new DelegateCommand(OpenAddDialog);
            AcceptDialogCommand = new DelegateCommand(AcceptDialog);
            CancelDialogCommand = new DelegateCommand(CancelDialog);

            BtnClickDeleteComplain = new DelegateCommand(BtnClickDeleteComplainCommand);           

            context.Complaints.Where(x => x.User.Id == UserId).Load();
            context.Districts.Load();
            ComplaintsList = context.Complaints.Local.ToObservableCollection();
            Districts = context.Districts.Local.ToObservableCollection();
          
        }

        public ICommand BtnClickDeleteComplain { get; }

        private void BtnClickDeleteComplainCommand(object obj)
        {           
            ComplaintsList.Remove(SelectedComplaint);
            context.SaveChanges();
        }

        #region DialogWindow

        public ICommand OpenAddDialogCommand { get; }
        public ICommand OpenEditDialogCommand { get; }
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

        private string _dialogHeader;
        public string DialogHeader
        {
            get
            {
                return _dialogHeader;
            }
            set { SetProperty(ref _dialogHeader, value); }
        }

        private void OpenAddDialog(object obj)
        {
            DialogContent = new ComplaintsDialog();
            DialogHeader = "Add complaint";
            BriefDescription = "";
            SelectedDistrict = null;
            IsAddDialog = true;
            IsDialogOpen = true;          
        }

        private void OpenEditDialog(object obj)
        {
            if (SelectedComplaint != null)
            {
                DialogContent = new ComplaintsDialog();
                DialogHeader = "Editing complaint";
                BriefDescription = SelectedComplaint.Description;
                SelectedDistrict = SelectedComplaint.District;
                IsAddDialog = false;
                IsDialogOpen = true;               
            }
        }

        private void CancelDialog(object obj) => IsDialogOpen = false;

        private void AcceptDialog(object obj)
        {
            if (IsAddDialog)
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
                    var complain = new Complaint
                    {
                        Date = DateTime.Now,
                        Description = BriefDescription,
                        Status = "Not"
                    };

                    var user = context.Users.FirstOrDefault(u => u.Id == UserId);
                    var district = context.Districts.FirstOrDefault(u => u.Id == SelectedDistrict.Id);

                    complain.User = user;
                    complain.District = district;

                    ComplaintsList.Add(complain);
                    context.SaveChanges();

                    IsDialogOpen = false;
                }
            }

            else
            {
                var complain = new Complaint
                {
                    Date = SelectedComplaint.Date,
                    Description = BriefDescription,
                    Status = "Not"
                };

                var user = context.Users.FirstOrDefault(u => u.Id == UserId);
                var district = context.Districts.FirstOrDefault(u => u.Id == SelectedDistrict.Id);

                complain.User = user;
                complain.District = district;

                ComplaintsList.Remove(SelectedComplaint);
                ComplaintsList.Add(complain);
                context.SaveChanges();

                IsDialogOpen = false;
            }   
            
        }

        #endregion

    }
}
