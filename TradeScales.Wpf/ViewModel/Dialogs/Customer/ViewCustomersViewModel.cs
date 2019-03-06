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
    public class ViewCustomersViewModel : BaseViewModel
    {
        #region fields

        private static IDialogsService _dialogService = ServiceLocator.Instance.GetService<IDialogsService>();
        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Customer> _customersRepository = BootStrapper.Resolve<IEntityBaseRepository<Customer>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public ViewCustomersViewModel()
        {
            NotXClosed = false;
            Customers = new ObservableCollection<CustomerViewModel>(Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(_customersRepository.GetAll().OrderBy(x => x.Code)));
        }

        #endregion

        #region Properties

        public bool NotXClosed { get; set; }

        private ObservableCollection<CustomerViewModel> _Customers;
        public ObservableCollection<CustomerViewModel> Customers
        {
            get { return _Customers; }
            set
            {
                _Customers = value;
                OnPropertyChanged("Customers");
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
                    _EditCommand = new RelayCommand<CustomerViewModel>(EditCustomer);
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

        private void EditCustomer(CustomerViewModel customer)
        {
            try
            {
                _dialogService.ShowAddCustomerDialog(true, customer.ID, customer.Code, customer.Name);
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
