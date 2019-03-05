using System;
using System.Text;
using System.Windows.Input;
using TradeScales.Services;
using TradeScales.Wpf.Properties;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Dialogs
{
    public class ActivateViewModel : BaseViewModel
    {
        #region fields

        private const string _uniqueIdentifierKey = "072A0744-9F3B-46A5-B79D-B32071185E05";

        #endregion

        #region Constructor

        public ActivateViewModel()
        {
            EncryptUniqueIdentifier();
        }

        #endregion

        #region Properties

        public string ExplainLicenseProcessMessage
        {
            get { return BuildExplainProcessMessage(); }
        }

        public string StepOneMessage
        {
            get { return BuildStepOneMessage(); }
        }

        private string _EncryptedUniqueIdentifier;
        public string EncryptedUniqueIdentifier
        {
            get { return _EncryptedUniqueIdentifier; }
            set
            {
                if (_EncryptedUniqueIdentifier != value)
                {
                    _EncryptedUniqueIdentifier = value;
                    OnPropertyChanged("EncryptedUniqueIdentifier");
                }
            }
        }

        public string StepTwoMessage
        {
            get { return BuildStepTwoMessage(); }
        }

        private string _EncryptedProductExpireDate;
        public string EncryptedProductExpireDate
        {
            get { return _EncryptedProductExpireDate; }
            set
            {
                if (_EncryptedProductExpireDate != value)
                {
                    _EncryptedProductExpireDate = value;
                    OnPropertyChanged("EncryptedProductExpireDate");
                }
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
                    _OkCommand = new MVVMRelayCommand(execute => { OK(); });
                }
                return _OkCommand;
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

        private void EncryptUniqueIdentifier()
        {
            var uniqueIdentifier = $"{Environment.MachineName}+{Environment.ProcessorCount}+{Environment.OSVersion}";
            EncryptedUniqueIdentifier = LicenseKeyService.Encrypt(uniqueIdentifier, _uniqueIdentifierKey);
        }

        private void OK()
        {
            if (!string.IsNullOrEmpty(EncryptedProductExpireDate))
            {
                Settings.Default.ProductExpireDate = EncryptedProductExpireDate;
                Settings.Default.Save();
            }

            OnRequestClose();
        }

        private string BuildExplainProcessMessage()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Insert message explaining license process here");        
            return builder.ToString();
        }

        private string BuildStepOneMessage()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Insert message explaining step one here");
            return builder.ToString();

        }

        private string BuildStepTwoMessage()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Insert message explaining step two here");
            return builder.ToString();
        }
        #endregion

    }
}
