using System;
using System.ComponentModel;
using System.Windows;
using WBS.Services;

namespace WBS.LicenseGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            UniqueIdentiferKey = "072A0744-9F3B-46A5-B79D-B32071185E05";
        }

        #endregion

        #region Properties

        private DateTime _ProductExpireDate;
        public DateTime ProductExpireDate
        {
            get { return _ProductExpireDate; }
            set
            {
                if (_ProductExpireDate != value)
                {
                    _ProductExpireDate = value;
                    OnPropertyChanged("ProductExpireDate");
                }
            }
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

        private string _UniqueIdentifierKey;
        public string UniqueIdentiferKey
        {
            get { return _UniqueIdentifierKey; }
            set
            {
                if (_UniqueIdentifierKey != value)
                {
                    _UniqueIdentifierKey = value;
                    OnPropertyChanged("UniqueIdentiferKey");
                }
            }
        }

        private string _EncryptedProductExpiryDate;
        public string EncryptedProductExpiryDate
        {
            get { return _EncryptedProductExpiryDate; }
            set
            {
                if (_EncryptedProductExpiryDate != value)
                {
                    _EncryptedProductExpiryDate = value;
                    OnPropertyChanged("EncryptedProductExpiryDate");
                }
            }
        }

        #endregion

        #region Private Methods
      
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var productEncryptionKey = LicenseKeyService.Decrypt(EncryptedUniqueIdentifier, UniqueIdentiferKey);
            EncryptedProductExpiryDate = LicenseKeyService.Encrypt(ProductExpireDate.ToString(), productEncryptionKey);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
