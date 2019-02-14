using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
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

        private string _SelectedPortName;
        public string SelectedPortName
        {
            get { return _SelectedPortName; }
            set
            {
                _SelectedPortName = value;
                OnPropertyChanged("SelectedPortName");
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


        private string _WeightResult;
        public string WeightResult
        {
            get { return _WeightResult; }
            set
            {
                _WeightResult = value;
                OnPropertyChanged("WeightResult");
            }
        }

        private string _WeightUnit;
        public string WeightUnit
        {
            get { return _WeightUnit; }
            set
            {
                _WeightUnit = value;
                OnPropertyChanged("WeightUnit");
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
                                WeightResult = "";
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

        #region Public Methods
        public void SetValues()
        {
            SelectedPortName = PortNames.First();
            BaudRate = 9600;
            DataBits = 8;
            Parity = Parity.None;
            StopBits = StopBits.One;
            WeightResult = "";
            IsReceiving = true;
        }

        public void CreateNewSerialPort()
        {
            if (SerialPort != null && SerialPort.IsOpen)
            {
                SerialPort.Close();
            }

            SerialPort = new SerialPort(SelectedPortName, BaudRate, Parity, DataBits, StopBits);
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            SerialPort.Open();
        }

        #endregion

        #region Private Methods

        private void InitialiseWeighBridge()
        {
            try
            {
                SetValues();
                CreateNewSerialPort();
            }
            catch(Exception exception)
            {
                MainViewModel.This.ShowExceptionMessageBox(exception);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                byte[] bytesReceived = ReadFromSerialPort(serialPort, 13);

                WeightResult = Encoding.ASCII.GetString(GetRange(bytesReceived, 2, 7));
                WeightUnit = Encoding.ASCII.GetString(GetRange(bytesReceived, 9, 2));
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

        public byte[] ReadFromSerialPort(SerialPort serialPort, int toRead)
        {
            byte[] buffer = new byte[toRead];
            int offset = 0;
            int read;

            while (toRead > 0 && (read = serialPort.Read(buffer, offset, toRead)) > 0)
            {
                offset += read;
                toRead -= read;
            }
            if (toRead > 0)
            {
                throw new EndOfStreamException();
            }

            return buffer;
        }

        public byte[] GetRange(byte[] array, int startIndex, int length)
        {
            byte[] subset = new byte[length];
            Array.Copy(array, startIndex, subset, 0, length);
            return subset;
        }
        #endregion

    }
}
