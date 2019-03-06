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
    public class AddVehicleViewModel : BaseViewModel
    {
        #region fields

        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Vehicle> _vehiclesRepository = BootStrapper.Resolve<IEntityBaseRepository<Vehicle>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public AddVehicleViewModel()
        {
            NotXClosed = false;
        }

        #endregion

        #region Properties

        public bool NotXClosed { get; set; }

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

        private string _Make;
        public string Make
        {
            get { return _Make; }
            set
            {
                _Make = value;
                OnPropertyChanged("Make");
            }
        }

        private string _Registration;
        public string Registration
        {
            get { return _Registration; }
            set
            {
                _Registration = value;
                OnPropertyChanged("Registration");
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
            if (_vehiclesRepository.VehicleExists(_Make, _Registration))
            {
                throw new ArgumentException("Vehicle already exists");
            }
            else
            {
                if (!string.IsNullOrEmpty(_Code) && !string.IsNullOrEmpty(_Make) && !string.IsNullOrEmpty(_Registration))
                {
                    VehicleViewModel newVehicle = new VehicleViewModel() { Code = _Code, Make = _Make, Registration = _Registration };
                    Vehicle vehicle = new Vehicle();
                    vehicle.UpdateVehicle(newVehicle);

                    _vehiclesRepository.Add(vehicle);
                    _unitOfWork.Commit();

                    Code = "";
                    _Make = "";
                    _Registration = "";

                    _messageBoxService.ShowMessageBox($"Successfully added new vehicle {newVehicle.Code}", "Success", MessageBoxButton.OK);
                    MainViewModel.This.StatusMessage = $"Successfully added new vehicle {newVehicle.Code}";
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
