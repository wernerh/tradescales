using System;
using System.Windows;
using System.Windows.Input;
using TradeScales.Data.Extensions;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Services.Abstract;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Dialogs
{
    public class AddUserViewModel : BaseViewModel
    {
        #region fields

        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();
        private static IMembershipService _membershipService = BootStrapper.Resolve<IMembershipService>();

        private IEntityBaseRepository<User> _usersRepository = BootStrapper.Resolve<IEntityBaseRepository<User>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public AddUserViewModel(bool isEditUser, int id = -1, string username = null, string email = null)
        {
            NotXClosed = false;
            DialogTitle = "Add User";
            ButtonTitle = "Add";

            if (isEditUser)
            {
                IsEditUser = true;
                DialogTitle = "Edit User";
                ButtonTitle = "Update";
                Id = id;
                Username = username;
                Email = email;
            }
        }

        #endregion

        #region Properties

        public bool IsEditUser { get; set; }

        public bool NotXClosed { get; set; }


        private string _DialogTitle;
        public string DialogTitle
        {
            get { return _DialogTitle; }
            set
            {
                _DialogTitle = value;
                OnPropertyChanged("DialogTitle");
            }
        }

        private string _ButtonTitle;
        public string ButtonTitle
        {
            get { return _ButtonTitle; }
            set
            {
                _ButtonTitle = value;
                OnPropertyChanged("ButtonTitle");
            }
        }

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                OnPropertyChanged("Username");
            }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged("Password");
            }
        }

        #endregion

        #region Commands

        private ICommand _OkCommand;
        public ICommand OkCommand
        {
            get
            {
                if (_OkCommand == null)
                {
                    _OkCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            if (IsEditUser)
                            {
                                EditUser();
                            }
                            else
                            {
                                AddUser();
                            }
                        }
                        catch (Exception exception)
                        {
                            _messageBoxService.ShowExceptionMessageBox(exception, "Error", MessageBoxImage.Error);
                        }
                    });
                }
                return _OkCommand;
            }
        }

        private ICommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            Cancel();
                        }
                        catch (Exception exception)
                        {
                            _messageBoxService.ShowExceptionMessageBox(exception, "Error", MessageBoxImage.Error);
                        }
                    });
                }
                return _CancelCommand;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the application dialog be closed
        /// </summary>
        public event EventHandler RequestClose;

        #endregion

        #region Private Methods

        private void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        public void AddUser()
        {
            if (_usersRepository.UserExists(_Email, _Username))
            {
                throw new ArgumentException("User already exists");
            }
            else
            {
                if (!string.IsNullOrEmpty(_Username) && !string.IsNullOrEmpty(_Email) && !string.IsNullOrEmpty(_Password))
                {

                    var newUser = _membershipService.CreateUser(_Username, _Email, _Password, new int[] { 2 });

                    Username = "";
                    Email = "";
                    Password = "";

                    _messageBoxService.ShowMessageBox($"Successfully added new user {newUser.Username}", "Success", MessageBoxButton.OK);
                    MainViewModel.This.StatusMessage = $"Successfully added new user {newUser.Username}";
                    MainViewModel.This.ReloadEntities();
                }
            }
        }

        private void EditUser()
        {
            if (!string.IsNullOrEmpty(_Username) && !string.IsNullOrEmpty(_Email))
            {
                User user = _usersRepository.GetSingle(_Id);
                user.Username = Username;
                user.Email = Email;

                _unitOfWork.Commit();
            
                MainViewModel.This.StatusMessage = $"Successfully updated customer {user.Username}";
                MainViewModel.This.ReloadEntities();

                NotXClosed = true;
                OnRequestClose();
            }
        }

        private void Cancel()
        {
            NotXClosed = true;
            OnRequestClose();
        }

        #endregion

    }
}
