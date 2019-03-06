using System;
using System.Windows;
using System.Windows.Input;
using TradeScales.Data.Extensions;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Wpf.Infrastructure.Extensions;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Dialogs
{
    public class AddDriverViewModel : BaseViewModel
    {
        #region fields

        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Driver> _driversRepository = BootStrapper.Resolve<IEntityBaseRepository<Driver>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public AddDriverViewModel(bool isEditDriver, int id = -1, string code = null, string firstname = null, string lastname = null)
        {
            NotXClosed = false;
            DialogTitle = "Add Driver";
            ButtonTitle = "Add";

            if (isEditDriver)
            {
                IsEditDriver = true;
                DialogTitle = "Edit Driver";
                ButtonTitle = "Update";
                Id = id;
                Code = code;
                FirstName = firstname;
                LastName = lastname;
            }
        }

        #endregion

        #region Properties

        public bool IsEditDriver { get; set; }

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

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                _Code = value;
                OnPropertyChanged("Code");
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged("LastName");
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
                            if (IsEditDriver)
                            {
                                EditDriver();
                            }
                            else
                            {
                                AddDriver();
                            }
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

        public void AddDriver()
        {
            if (_driversRepository.DriverExists(_FirstName, _LastName))
            {
                throw new ArgumentException("Driver already exists");
            }
            else
            {
                if (!string.IsNullOrEmpty(_Code) && !string.IsNullOrEmpty(_FirstName) && !string.IsNullOrEmpty(_LastName))
                {
                    DriverViewModel newDriver = new DriverViewModel() { Code = _Code, FirstName = _FirstName, LastName = _LastName };
                    Driver driver = new Driver();
                    driver.UpdateDriver(newDriver);

                    _driversRepository.Add(driver);
                    _unitOfWork.Commit();

                    Code = "";
                    FirstName = "";
                    LastName = "";

                    _messageBoxService.ShowMessageBox($"Successfully added new driver {newDriver.Code}", "Success", MessageBoxButton.OK);
                    MainViewModel.This.StatusMessage = $"Successfully added new driver {newDriver.Code}";
                    MainViewModel.This.ReloadEntities();
                }
            }
        }

        public void EditDriver()
        {
            if (!string.IsNullOrEmpty(_Code) && !string.IsNullOrEmpty(_FirstName) && !string.IsNullOrEmpty(_LastName))
            {
                DriverViewModel newDriver = new DriverViewModel() { Code = _Code, FirstName = _FirstName, LastName = _LastName };
                Driver driver = _driversRepository.GetSingle(_Id);
                driver.UpdateDriver(newDriver);

                _unitOfWork.Commit();
             

                MainViewModel.This.StatusMessage = $"Successfully updated driver {newDriver.Code}";
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
