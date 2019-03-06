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
    public class AddProductViewModel : BaseViewModel
    {
        #region fields

        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Product> _productsRepository = BootStrapper.Resolve<IEntityBaseRepository<Product>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public AddProductViewModel(bool isEditProduct, int id = -1, string code = null, string name = null)
        {
            NotXClosed = false;
            DialogTitle = "Add Product";
            ButtonTitle = "Add";

            if (isEditProduct)
            {
                IsEditProduct = true;
                DialogTitle = "Edit Product";
                ButtonTitle = "Update";
                Id = id;
                Code = code;
                Name = name;
            }
        }

        #endregion

        #region Properties

        public bool IsEditProduct { get; set; }


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
                            if (IsEditProduct)
                            {
                                EditProduct();
                            }
                            else
                            {
                                AddProduct();
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

        public void AddProduct()
        {
            if (_productsRepository.ProductExists(_Code, _Name))
            {
                throw new ArgumentException("Product already exists");
            }
            else
            {
                if (!string.IsNullOrEmpty(_Code) && !string.IsNullOrEmpty(_Name))
                {
                    ProductViewModel newProduct = new ProductViewModel() { Code = _Code, Name = _Name };
                    Product product = new Product();
                    product.UpdateProduct(newProduct);

                    _productsRepository.Add(product);
                    _unitOfWork.Commit();

                    Code = "";
                    Name = "";

                    _messageBoxService.ShowMessageBox($"Successfully added new product {product.Code}", "Success", MessageBoxButton.OK);
                    MainViewModel.This.StatusMessage = $"Successfully added new product {product.Code}";
                    MainViewModel.This.ReloadEntities();
                }
            }
        }

        public void EditProduct()
        {
            if (!string.IsNullOrEmpty(_Code) && !string.IsNullOrEmpty(_Name))
            {
                ProductViewModel newProduct = new ProductViewModel() { Code = _Code, Name = _Name };
                Product product = _productsRepository.GetSingle(_Id);
                product.UpdateProduct(newProduct); ;

                _unitOfWork.Commit();
               
                MainViewModel.This.StatusMessage = $"Successfully updated product {newProduct.Code}";
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
