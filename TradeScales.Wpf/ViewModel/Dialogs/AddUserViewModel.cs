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

        #endregion

        #region Constructor

        public AddUserViewModel()
        {
            NotXClosed = false;
        }

        #endregion

        #region Properties

        public bool NotXClosed { get; set; }

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
                            Ok();
                        }
                        catch(Exception exception)
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

        public void Ok()
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

        private void Cancel()
        {
            NotXClosed = true;
            OnRequestClose();
        }

        #endregion

    }
}
