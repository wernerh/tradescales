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
            WeightResult = "";
            IsReceiving = true;
        }

        public void CreateNewSerialPort()
        {
            try
            {
                if (SerialPort != null && SerialPort.IsOpen)
                {
                    SerialPort.Close();
                }

                var selectedPortName = Properties.Settings.Default.SelectedPortName;
                var baudRate = Properties.Settings.Default.BaudRate;
                var dataBits = Properties.Settings.Default.DataBits;
                var parity = (Parity)Properties.Settings.Default.Parity;
                var stopBits = (StopBits)Properties.Settings.Default.StopBits;
                var serialports = SerialPort.GetPortNames();

                if (!string.IsNullOrEmpty(selectedPortName) && serialports.Contains(selectedPortName))
                {
                    SerialPort = new SerialPort(selectedPortName, baudRate, parity, dataBits, stopBits);
                    SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                    SerialPort.Open();
                }
            }
            catch (Exception exception)
            {
                MainViewModel.This.ShowExceptionMessageBox(exception);
            }
        }

        #endregion

        #region Private Methods

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
            var weightReading = double.Parse(WeightResult);
            MainViewModel.This.ActiveDocument.UpdateWeight(weightReading, IsReceiving);
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
