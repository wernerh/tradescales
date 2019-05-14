using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Entities;
using WBS.Wpf.Model;
using WBS.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = WBS.Wpf.Model.RelayCommand;

namespace WBS.Wpf.ViewModel.Dialogs
{
    public class ViewDriversViewModel : BaseViewModel
    {
        #region fields

        private static IDialogsService _dialogService = ServiceLocator.Instance.GetService<IDialogsService>();
        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Driver> _driversRepository = BootStrapper.Resolve<IEntityBaseRepository<Driver>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public ViewDriversViewModel()
        {
            NotXClosed = false;
            LoadDrivers();
        }

        #endregion

        #region Properties

        public bool NotXClosed { get; set; }

        private ObservableCollection<DriverViewModel> _Drivers;
        public ObservableCollection<DriverViewModel> Drivers
        {
            get { return _Drivers; }
            set
            {
                _Drivers = value;
                OnPropertyChanged("Drivers");
            }
        }

        #endregion

        #region Commands

        private ICommand _EditCommand;     
        public ICommand EditCommand
        {
            get
            {
                if (_EditCommand == null)
                {
                    _EditCommand = new RelayCommand<DriverViewModel>(EditDriver);
                }
                return _EditCommand;
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

        private void EditDriver(DriverViewModel driver)
        {
            try
            {
                _dialogService.ShowAddDriverDialog(true, driver.ID, driver.Code, driver.FirstName, driver.LastName);
                LoadDrivers();
            }
            catch (Exception ex)
            {
                MainViewModel.This.ShowExceptionMessageBox(ex);
            }
        }

        private void Cancel()
        {
            NotXClosed = true;
            OnRequestClose();
        }

        private void LoadDrivers()
        {
            Drivers = new ObservableCollection<DriverViewModel>(Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(_driversRepository.GetAll().OrderBy(x => x.Code)));
        }

        #endregion

    }
}
