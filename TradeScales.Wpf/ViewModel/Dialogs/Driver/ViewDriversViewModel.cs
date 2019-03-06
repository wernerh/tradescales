using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Dialogs
{
    public class ViewDriversViewModel : BaseViewModel
    {
        #region fields

        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Driver> _driversRepository = BootStrapper.Resolve<IEntityBaseRepository<Driver>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public ViewDriversViewModel()
        {
            NotXClosed = false;
            Drivers = new ObservableCollection<DriverViewModel>(Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(_driversRepository.GetAll().OrderBy(x => x.Code)));
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
                //MainViewModel.This.OpenEditTicket(ticket);
                throw new ArgumentException("On the backlog");
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

        #endregion

    }
}
