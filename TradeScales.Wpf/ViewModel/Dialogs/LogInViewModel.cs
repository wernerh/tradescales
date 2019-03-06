using System;
using System.Windows.Input;
using TradeScales.Services;
using TradeScales.Services.Abstract;
using TradeScales.Services.Utilities;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Properties;
using TradeScales.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Dialogs
{
    /// <summary>
    /// Log in view model
    /// </summary>
    public class LogInViewModel : BaseViewModel
    {
        #region fields

        private static IDialogsService _DialogService = ServiceLocator.Instance.GetService<IDialogsService>();
        private static IMembershipService _membershipService = BootStrapper.Resolve<IMembershipService>();

        private const string _uniqueIdentifierKey = "072A0744-9F3B-46A5-B79D-B32071185E05";

        #endregion

        #region Properties

        public string ImagePath
        {
            get { return Settings.Default.LogInLogo; }
        }

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        private bool _UserCanActivate;
        public bool UserCanActivate
        {
            get { return _UserCanActivate; }
            set
            {
                if (_UserCanActivate != value)
                {
                    _UserCanActivate = value;
                    OnPropertyChanged("UserCanActivate");
                }
            }
        }

        private bool _UserCanLogIn;
        public bool UserCanLogIn
        {
            get { return _UserCanLogIn; }
            set
            {
                if (_UserCanLogIn != value)
                {
                    _UserCanLogIn = value;
                    OnPropertyChanged("UserCanLogIn");
                }
            }
        }

        private string _Error;
        public string Error
        {
            get { return _Error; }
            set
            {
                if (_Error != value)
                {
                    _Error = value;
                    OnPropertyChanged("Error");
                }
            }
        }

        public MembershipContext LoggedInUserContext { get; set; }

        #endregion

        #region Constructor

        public LogInViewModel()
        {
            CheckProductExpireDate();
        }

        #endregion

        #region Commands

        private ICommand _LogInCommand;
        public ICommand LogInCommand
        {
            get
            {
                if (_LogInCommand == null)
                {
                    _LogInCommand = new MVVMRelayCommand(execute => { LogIn(); });
                }
                return _LogInCommand;
            }
        }

        private ICommand _ActivateCommand;
        public ICommand ActivateCommand
        {
            get
            {
                if (_ActivateCommand == null)
                {
                    _ActivateCommand = new MVVMRelayCommand(execute => { Activate(); });
                }
                return _ActivateCommand;
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

        private void LogIn()
        {
            try
            {
                MembershipContext _userContext = _membershipService.ValidateUser(Username.Trim(), Password.Trim());

                if (_userContext.User != null)
                {
                    LoggedInUserContext = _userContext;
                    OnRequestClose();
                }
                else
                {
                    throw new ArgumentException("Log in failed");
                }
            }
            catch
            {
                Error = "Login failed! Please try again";
            }
        }

        private void CheckProductExpireDate()
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.Default.ProductExpireDate))
                {
                    var productExpireDate = GetProductDemoExpireDate();
                    if (DateTime.Now < productExpireDate)
                    {
                        // Active license - Show log in / hide activate now
                        UserCanLogIn = true;
                        UserCanActivate = false;
                        Error = "";
                    }
                }
                else
                {
                    var productDemoExpireDate = GetProductDemoExpireDate();
                    if (DateTime.Now > productDemoExpireDate)
                    {
                        // Demo expired - Hide log in / Show activate
                        UserCanLogIn = false;
                        UserCanActivate = true;
                        Error = "Your version of TradeScales has expired, please activate now";
                    }
                    else
                    {
                        // Demo period - show log in / Show activate now
                        UserCanLogIn = true;
                        UserCanActivate = true;
                        Error = $"You have {(productDemoExpireDate - DateTime.Now).Days} days left to activate TradeScales";
                    }
                }
            }
            catch(Exception exception)
            {
                UserCanLogIn = false;
                UserCanActivate = true;
                Error = "Your version of TradeScales has expired, please activate now";
            }
        }

        private DateTime GetProductDemoExpireDate()
        {
            string decryptedProductDemoExpireDate = LicenseKeyService.Decrypt(Settings.Default.ProductDemoExpireDate, _uniqueIdentifierKey);
            return DateTime.Parse(decryptedProductDemoExpireDate);
        }

        private DateTime GetProductExpireDate()
        {
            var productEncryptionKey = $"{Environment.MachineName}+{Environment.ProcessorCount}+{Environment.OSVersion}";
            var decryptedProductExpireDate = LicenseKeyService.Decrypt(Settings.Default.ProductExpireDate, productEncryptionKey);
            return DateTime.Parse(decryptedProductExpireDate);
        }

        private bool CanLogIn()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void Activate()
        {
            _DialogService.ShowActivateDialog();
            CheckProductExpireDate();
        }

        #endregion

    }
}
