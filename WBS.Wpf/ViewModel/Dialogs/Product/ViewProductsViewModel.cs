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
    public class ViewProductsViewModel : BaseViewModel
    {
        #region fields

        private static IDialogsService _dialogService = ServiceLocator.Instance.GetService<IDialogsService>();
        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Product> _productsRepository = BootStrapper.Resolve<IEntityBaseRepository<Product>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public ViewProductsViewModel()
        {
            NotXClosed = false;
            LoadProducts();
        }

        #endregion

        #region Properties

        public bool NotXClosed { get; set; }

        private ObservableCollection<ProductViewModel> _Products;
        public ObservableCollection<ProductViewModel> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged("Products");
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
                    _EditCommand = new RelayCommand<ProductViewModel>(EditProduct);
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

        private void EditProduct(ProductViewModel product)
        {
            try
            {
                _dialogService.ShowAddProductDialog(true, product.ID, product.Code, product.Name);
                LoadProducts();
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

        private void LoadProducts()
        {
            Products = new ObservableCollection<ProductViewModel>(Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productsRepository.GetAll().OrderBy(x => x.Code)));
        }

        #endregion

    }
}
