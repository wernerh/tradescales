using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace TradeScales.LicenseGenerator
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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            FirstName = "Werner Hurter";
            ExpiryDate = DateTime.Now;
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private DateTime _ExpiryDate;
        public DateTime ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }


        private List<string> _LicenseTypes;
        public List<string> LicenseTypes
        {
            get
            {
                if (_LicenseTypes == null)
                {
                    _LicenseTypes = new List<string>();
                    foreach (var item in Enum.GetNames(typeof(LicenseType)))
                    {
                        _LicenseTypes.Add(item.ToString());
                    }

                }
                return _LicenseTypes;
            }
            set
            {
                if (_LicenseTypes != value)
                {
                    _LicenseTypes = value;
                    OnPropertyChanged("LicenseTypes");
                }
            }
        }

        private string _SelectedLicenseType;
        public string SelectedLicenseType
        {
            get { return _SelectedLicenseType; }
            set
            {
                if (_SelectedLicenseType != value)
                {
                    _SelectedLicenseType = value;
                    OnPropertyChanged("SelectedLicenseTypes");
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
