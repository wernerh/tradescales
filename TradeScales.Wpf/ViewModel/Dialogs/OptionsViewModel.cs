using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows.Input;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Dialogs
{
    /// <summary>
    /// Options view model
    /// </summary>
    public class OptionsViewModel : BaseViewModel
    {

        #region Properties

        private ObservableCollection<string> _ThemeOptions;
        /// <summary>
        /// Different application themes
        /// Options: BaseDark, BaseLight and Metro
        /// </summary>
        public ObservableCollection<string> ThemeOptions
        {
            get { return _ThemeOptions; }
            set
            {
                if (_ThemeOptions != value)
                {
                    _ThemeOptions = value;
                    OnPropertyChanged("ThemeOptions");
                }
            }
        }

        private ObservableCollection<string> _ThemeAccents;
        /// <summary>
        /// Different application accents
        /// Options: Red, Green, Blue, Purple, Orange, Lime, Emerald, Teal, Cyan, Cobalt, Indigo, Violet, Pink, Magenta, Metro, Crimson, Amber, Yellow, Brown, Olive, Steel, Mauve, Taupe and Sienna
        /// </summary>
        public ObservableCollection<string> ThemeAccents
        {
            get { return _ThemeAccents; }
            set
            {
                if (_ThemeAccents != value)
                {
                    _ThemeAccents = value;
                    OnPropertyChanged("ThemeAccents");
                }
            }
        }

        /// <summary>
        /// Selected application theme
        /// </summary>
        public string Theme
        {
            get { return Properties.Settings.Default.Theme; }
            set
            {
                if (Properties.Settings.Default.Theme != value)
                {
                    Properties.Settings.Default.Theme = value;

                    if (value == "BaseDark")
                    {
                        Properties.Settings.Default.WorkBenchTheme = "Dark";
                    }
                    else
                    {
                        Properties.Settings.Default.WorkBenchTheme = "Metro";
                    }

                    OnPropertyChanged("Theme");
                }
            }
        }

        /// <summary>
        /// Selected theme accent
        /// </summary>
        public string ThemeAccent
        {
            get { return Properties.Settings.Default.ThemeAccent; }
            set
            {
                if (Properties.Settings.Default.ThemeAccent != value)
                {
                    Properties.Settings.Default.ThemeAccent = value;
                    OnPropertyChanged("ThemeAccent");
                }
            }
        }

        /// <summary>
        /// Show start page on start up
        /// </summary>
        public bool ShowStartPage
        {
            get { return Properties.Settings.Default.ShowStartPageOnStartup; }
            set
            {
                if (Properties.Settings.Default.ShowStartPageOnStartup != value)
                {
                    Properties.Settings.Default.ShowStartPageOnStartup = value;
                    OnPropertyChanged("ShowStartPage");
                }
            }
        }

        /// <summary>
        /// Flag that indicates how the window was closed 
        /// </summary>
        public bool NotXClosed { get; set; }

        private IEnumerable<string> _PortNames;
        public IEnumerable<string> PortNames
        {
            get
            {
                if (_PortNames == null)
                {
                    _PortNames = new List<string>(SerialPort.GetPortNames());
                }
                return _PortNames;
            }
            set
            {
                _PortNames = value;
            }
        }

        public string SelectedPortName
        {
            get { return Properties.Settings.Default.SelectedPortName; }
            set
            {
                Properties.Settings.Default.SelectedPortName = value;
                OnPropertyChanged("SelectedPortName");
            }
        }

        public int BaudRate
        {
            get { return Properties.Settings.Default.BaudRate; }
            set
            {
                Properties.Settings.Default.BaudRate = value;
                OnPropertyChanged("BaudRate");
            }
        }

        public int DataBits
        {
            get { return Properties.Settings.Default.DataBits; }
            set
            {
                Properties.Settings.Default.DataBits = value;
                OnPropertyChanged("DataBits");
            }
        }

        public Parity Parity
        {
            get { return (Parity)Properties.Settings.Default.Parity; }
            set
            {
                Properties.Settings.Default.Parity = (int)value;
                OnPropertyChanged("Parity");
            }
        }

        public StopBits StopBits
        {
            get { return (StopBits)Properties.Settings.Default.StopBits; }
            set
            {
                Properties.Settings.Default.StopBits = (int)value;
                OnPropertyChanged("StopBits");
            }
        }

        public string WeighBridgeCertificatesFolder
        {
            get { return Properties.Settings.Default.WeighBridgeCertificatesFolder; }
            set
            {
                Properties.Settings.Default.WeighBridgeCertificatesFolder = value;
                OnPropertyChanged("WeighBridgeCertificatesFolder");
            }
        }

        public string WeighBridgeCertificateLogo
        {
            get { return Properties.Settings.Default.WeighBridgeCertificateLogo; }
            set
            {
                Properties.Settings.Default.WeighBridgeCertificateLogo = value;
                OnPropertyChanged("WeighBridgeCertificateLogo");
            }
        }

        public string WeighBridgeCertificateTemplate
        {
            get { return Properties.Settings.Default.WeighBridgeCertificateTemplate; }
            set
            {
                Properties.Settings.Default.WeighBridgeCertificateTemplate = value;
                OnPropertyChanged("WeighBridgeCertificateTemplate");
            }
        }

        public string ReportsFolder
        {
            get { return Properties.Settings.Default.ReportsFolder; }
            set
            {
                Properties.Settings.Default.ReportsFolder = value;
                OnPropertyChanged("ReportsFolder");
            }
        }

        public string ReportLogo
        {
            get { return Properties.Settings.Default.ReportLogo; }
            set
            {
                Properties.Settings.Default.ReportLogo = value;
                OnPropertyChanged("ReportLogo");
            }
        }

        public string ReportTemplate
        {
            get { return Properties.Settings.Default.ReportTemplate; }
            set
            {
                Properties.Settings.Default.ReportTemplate = value;
                OnPropertyChanged("ReportTemplate");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the OptionsViewModel class
        /// </summary>
        public OptionsViewModel()
        {
            _ThemeOptions = new ObservableCollection<string>() { "BaseDark", "BaseLight" };
            _ThemeAccents = new ObservableCollection<string>() { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" };
            NotXClosed = false;
        }

        #endregion

        #region Commands

        private ICommand _DefaultCommand;
        /// <summary>
        /// Restore application to default theme settings
        /// </summary>
        public ICommand DefaultCommand
        {
            get
            {
                if (_DefaultCommand == null)
                {
                    _DefaultCommand = new MVVMRelayCommand(execute => { SetToDefault(); });
                }
                return _DefaultCommand;
            }
        }

        private ICommand _PreviewCommand;
        /// <summary>
        /// Preview theme change
        /// </summary>
        public ICommand PreviewCommand
        {
            get
            {
                if (_PreviewCommand == null)
                {
                    _PreviewCommand = new MVVMRelayCommand(execute => { Preview(); });
                }
                return _PreviewCommand;
            }
        }

        private ICommand _OkCommand;
        /// <summary>
        /// Accept and close the dialog box
        /// </summary>
        public ICommand OkCommand
        {
            get
            {
                if (_OkCommand == null)
                {
                    _OkCommand = new MVVMRelayCommand(execute => { Ok(); });
                }
                return _OkCommand;
            }
        }

        private ICommand _CancelCommand;
        /// <summary>
        /// Cancel and close dialog box
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new MVVMRelayCommand(execute => { Cancel(); });
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

        private void SetToDefault()
        {
            Theme = "BaseDark";
            ThemeAccent = "Steel";
            BaudRate = 9600;
            DataBits = 8;
            Parity = Parity.None;
            StopBits = StopBits.One;

        }

        public void Preview()
        {
            Properties.Settings.Default.Save();
            MainViewModel.This.ChangeAppTheme();
        }

        public void Ok()
        {
            NotXClosed = true;
            Properties.Settings.Default.Save();
            MainViewModel.This.ToolOne.CreateNewSerialPort();
            OnRequestClose();
        }

        private void Cancel()
        {
            NotXClosed = true;
            OnRequestClose();
        }

        #endregion

    }
}
