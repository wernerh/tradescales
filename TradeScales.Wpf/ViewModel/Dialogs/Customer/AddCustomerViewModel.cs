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
    public class AddCustomerViewModel : BaseViewModel
    {
        #region fields

        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Customer> _customersRepository = BootStrapper.Resolve<IEntityBaseRepository<Customer>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public AddCustomerViewModel(bool isEditCustomer, int id = -1, string code = null, string name = null)
        {
            NotXClosed = false;
            DialogTitle = "Add Customer";
            ButtonTitle = "Add";

            if (isEditCustomer)
            {
                IsEditCustomer = true;
                DialogTitle = "Edit Customer";
                ButtonTitle = "Update";
                Id = id;
                Code = code;
                Name = name;
            }
        }

        #endregion

        #region Properties

        public bool IsEditCustomer { get; set; }

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

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
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
                            if (IsEditCustomer)
                            {
                                EditCustomer();
                            }
                            else
                            {
                                AddCustomer();
                            }

                        }
                        catch (Exception exception)
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

        public void AddCustomer()
        {
            if (_customersRepository.CustomerExists(_Code))
            {
                throw new ArgumentException("Customer already exists");
            }
            else
            {
                if (!string.IsNullOrEmpty(_Code) && !string.IsNullOrEmpty(_Name))
                {
                    CustomerViewModel newCustomer = new CustomerViewModel() { Code = _Code, Name = _Name };
                    Customer customer = new Customer();
                    customer.UpdateCustomer(newCustomer);

                    _customersRepository.Add(customer);
                    _unitOfWork.Commit();

                    Code = "";
                    Name = "";

                    _messageBoxService.ShowMessageBox($"Successfully added new customer {newCustomer.Code}", "Success", MessageBoxButton.OK);
                    MainViewModel.This.StatusMessage = $"Successfully added new customer {newCustomer.Code}";
                    MainViewModel.This.ReloadEntities();
                }
            }
        }

        public void EditCustomer()
        {
            if (!string.IsNullOrEmpty(_Code) && !string.IsNullOrEmpty(_Name))
            {
                CustomerViewModel newCustomer = new CustomerViewModel() { Code = _Code, Name = _Name };
                Customer customer = _customersRepository.GetSingle(_Id);
                customer.UpdateCustomer(newCustomer);

                _unitOfWork.Commit();
                _messageBoxService.ShowMessageBox($"Successfully updated customer {newCustomer.Code}", "Success", MessageBoxButton.OK);
                MainViewModel.This.StatusMessage = $"Successfully updated customer {newCustomer.Code}";
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
