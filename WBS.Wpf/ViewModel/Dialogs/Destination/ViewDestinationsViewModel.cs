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
    public class ViewDestinationsViewModel : BaseViewModel
    {
        #region fields

        private static IDialogsService _dialogService = ServiceLocator.Instance.GetService<IDialogsService>();
        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Destination> _destinationsRepository = BootStrapper.Resolve<IEntityBaseRepository<Destination>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public ViewDestinationsViewModel()
        {
            NotXClosed = false;
            LoadDestinations();
        }

        #endregion

        #region Properties

        public bool NotXClosed { get; set; }

        private ObservableCollection<DestinationViewModel> _Destinations;
        public ObservableCollection<DestinationViewModel> Destinations
        {
            get { return _Destinations; }
            set
            {
                _Destinations = value;
                OnPropertyChanged("Destinations");
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
                    _EditCommand = new RelayCommand<DestinationViewModel>(EditDestination);
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

        private void EditDestination(DestinationViewModel destination)
        {
            try
            {
                _dialogService.ShowAddDestinationDialog(true, destination.ID, destination.Code, destination.Name);
                LoadDestinations();
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

        private void LoadDestinations()
        {
            Destinations = new ObservableCollection<DestinationViewModel>(Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(_destinationsRepository.GetAll().OrderBy(x => x.Code)));
        }

        #endregion

    }
}
