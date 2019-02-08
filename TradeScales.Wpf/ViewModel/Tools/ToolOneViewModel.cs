using System;
using System.IO.Ports;
using System.Windows.Input;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Tools
{
    /// <summary>
    /// Tool one viewmodel
    /// </summary>
    public class ToolOneViewModel : ToolViewModel
    {

        #region Fields

        public const string ToolContentID = "ToolOne";
        private static IMessageBoxService _MessageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        #endregion

        #region Properties

        private string _PortName;
        public string PortName
        {
            get { return _PortName; }
            set
            {
                _PortName = value;
                OnPropertyChanged("PortName");

                if (SerialPort != null)
                {
                    SerialPort.PortName = _PortName;
                }
            }
        }

        private int _BaudRate;
        public int BaudRate
        {
            get { return _BaudRate; }
            set
            {
                _BaudRate = value;
                OnPropertyChanged("BaudRate");

                if (SerialPort != null)
                {
                    SerialPort.BaudRate = _BaudRate;
                }
            }
        }

        private int _DataBits;
        public int DataBits
        {
            get { return _DataBits; }
            set
            {
                _DataBits = value;
                OnPropertyChanged("DataBits");

                if (SerialPort != null)
                {
                    SerialPort.DataBits = _DataBits;
                }
            }
        }

        private Parity _Parity;
        public Parity Parity
        {
            get { return _Parity; }
            set
            {
                _Parity = value;
                OnPropertyChanged("Parity");

                if (SerialPort != null)
                {
                    SerialPort.Parity = _Parity;
                }
            }
        }

        private StopBits _StopBits;
        public StopBits StopBits
        {
            get { return _StopBits; }
            set
            {
                _StopBits = value;
                OnPropertyChanged("StopBits");
            }
        }

        private SerialPort _SerialPort;
        public SerialPort SerialPort
        {
            get { return _SerialPort; }
            set
            {
                _SerialPort = value;
                OnPropertyChanged("SerialPort");
            }
        }

        private string _ActiveDocument;
        public string ActiveDocument
        {
            get { return _ActiveDocument; }
            set
            {
                _ActiveDocument = value;
                OnPropertyChanged("ActiveDocument");
            }
        }

        private bool _IsReceiving;
        public bool IsReceiving
        {
            get { return _IsReceiving; }
            set
            {
                _IsReceiving = value;
                OnPropertyChanged("IsReceiving");
            }
        }


        private string _Reading;
        public string Reading
        {
            get { return _Reading; }
            set
            {
                _Reading = value;
                OnPropertyChanged("Reading");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ToolOneViewModel class
        /// </summary>
        public ToolOneViewModel()
            : base("Weigh Bridge")
        {
            ContentID = ToolContentID;
            SetDefaultValues();
            CreateNewSerialPort();
        }

        #endregion

        #region Commands

        private ICommand _ClearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_ClearCommand == null)
                {
                    _ClearCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                Reading = "";
                            }
                            catch (Exception ex)
                            {
                                MainViewModel.This.ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _ClearCommand;
            }
        }

        private ICommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                SaveWeight();
                            }
                            catch (Exception exception)
                            {
                                MainViewModel.This.ShowExceptionMessageBox(exception);
                            }
                        }, canExecute => CanSaveReading());
                }
                return _SaveCommand;
            }
        }

        #endregion

        #region Private Methods

        private void SetDefaultValues()
        {
            PortName = "COM3";
            BaudRate = 9600;
            DataBits = 8;
            Parity = Parity.None;
            StopBits = StopBits.One;
            Reading = "1000";
            IsReceiving = true;
        }

        private void CreateNewSerialPort()
        {
            SerialPort = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            SerialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                string dataReceived = serialPort.ReadExisting();
            }
            catch (Exception exception)
            {
                MainViewModel.This.ShowExceptionMessageBox(exception);
            }
        }

        private void UpdateStatusMessage(string message)
        {
            MainViewModel.This.StatusMessage = message;
        }

        private void SaveWeight()
        {
            MainViewModel.This.StatusMessage = $"Weight Saved {DateTime.Now}";
        }

        private bool CanSaveReading()
        {
            return MainViewModel.This.ActiveDocument.ContentID.Contains("New") || MainViewModel.This.ActiveDocument.ContentID.Contains("Edit");
        }

        #endregion

    }
}
