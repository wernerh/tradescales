using System;
using System.Windows;
using System.Windows.Input;
using WBS.Data.Extensions;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Entities;
using WBS.Wpf.Infrastructure.Extensions;
using WBS.Wpf.Model;
using WBS.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = WBS.Wpf.Model.RelayCommand;

namespace WBS.Wpf.ViewModel.Dialogs
{
    public class AddVehicleViewModel : BaseViewModel
    {
        #region fields

        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Vehicle> _vehiclesRepository = BootStrapper.Resolve<IEntityBaseRepository<Vehicle>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public AddVehicleViewModel(bool isEditVehicle, int id = -1, string code = null, string make = null, string registration = null)
        {
            NotXClosed = false;
            DialogTitle = "Add Vehicle";
            ButtonTitle = "Add";

            if (isEditVehicle)
            {
                IsEditVehicle = true;
                DialogTitle = "Edit Vehicle";
                ButtonTitle = "Update";
                Id = id;
                Code = code;
                Make = make;
                Registration = registration;
            }
        }

        #endregion

        #region Properties

        public bool IsEditVehicle { get; set; }

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
                            if (IsEditVehicle)
                            {
                                EditVehicle();
                            }
                            else
                            {
                                AddVehicle();
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

        public void AddVehicle()
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

        public void EditVehicle()
        {
            if (!string.IsNullOrEmpty(_Code) && !string.IsNullOrEmpty(_Make) && !string.IsNullOrEmpty(_Registration))
            {
                VehicleViewModel newVehicle = new VehicleViewModel() { Code = _Code, Make = _Make, Registration = _Registration };
                Vehicle vehicle = _vehiclesRepository.GetSingle(_Id);
                vehicle.UpdateVehicle(newVehicle);

                _unitOfWork.Commit();
               
                MainViewModel.This.StatusMessage = $"Successfully updated vehicle {newVehicle.Code}";
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
