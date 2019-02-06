
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel
{
    /// <summary>
    /// New ticket view model
    /// </summary>
    public class EditTicketViewModel : DocumentViewModel
    {

        #region Fields

        public const string ToolContentID = "EditTicketViewModel";
        private static IMessageBoxService messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();
        private readonly IEntityBaseRepository<Ticket> _ticketsRepository;
        private TicketViewModel _ticket;

        #endregion

        #region Properties
        private string _TicketNumber;
        public string TicketNumber
        {
            get { return _TicketNumber; }
            set
            {
                _TicketNumber = value;
                OnPropertyChanged("TicketNumber");
            }
        }

        private string _TimeIn;
        public string TimeIn
        {
            get { return _TimeIn; }
            set
            {
                _TimeIn = value;
                OnPropertyChanged("TimeIn");
            }
        }

        private string _TimeOut;
        public string TimeOut
        {
            get { return _TimeOut; }
            set
            {
                _TimeOut = value;
                OnPropertyChanged("TimeOut");
            }
        }

        public ObservableCollection<HaulierViewModel> Hauliers { get; set; }

        private HaulierViewModel _SelectedHaulier;
        public HaulierViewModel SelectedHaulier
        {
            get { return _SelectedHaulier; }
            set
            {
                _SelectedHaulier = value;
                OnPropertyChanged("SelectedHaulier");
            }
        }

        public ObservableCollection<CustomerViewModel> Customers { get; set; }

        private CustomerViewModel _SelectedCustomer;
        public CustomerViewModel SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        public ObservableCollection<DestinationViewModel> Destinations { get; set; }

        private DestinationViewModel _SelectedDestination;
        public DestinationViewModel SelectedDestination
        {
            get { return _SelectedDestination; }
            set
            {
                _SelectedDestination = value;
                OnPropertyChanged("SelectedDestination");
            }
        }

        public ObservableCollection<ProductViewModel> Products { get; set; }

        private ProductViewModel _SelectedProduct;
        public ProductViewModel SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public ObservableCollection<DriverViewModel> Drivers { get; set; }

        private DriverViewModel _SelectedDriver;
        public DriverViewModel SelectedDriver
        {
            get { return _SelectedDriver; }
            set
            {
                _SelectedDriver = value;
                OnPropertyChanged("SelectedDriver");
            }
        }

        private string _OrderNumber;
        public string OrderNumber
        {
            get { return _OrderNumber; }
            set
            {
                _OrderNumber = value;
                OnPropertyChanged("OrderNumber");
            }
        }

        private string _DeliveryNumber;
        public string DeliveryNumber
        {
            get { return _DeliveryNumber; }
            set
            {
                _DeliveryNumber = value;
                OnPropertyChanged("DeliveryNumber");
            }
        }

        private string _SealTo;
        public string SealTo
        {
            get { return _SealTo; }
            set
            {
                _SealTo = value;
                OnPropertyChanged("SealTo");
            }
        }

        private string _SealFrom;
        public string SealFrom
        {
            get { return _SealFrom; }
            set
            {
                _SealFrom = value;
                OnPropertyChanged("SealFrom");
            }
        }

        private double _TareWeight;
        public double TareWeight
        {
            get { return _TareWeight; }
            set
            {
                _TareWeight = value;
                OnPropertyChanged("TareWeight");
                UpdateNettWeight();
            }
        }

        private double _GrossWeight;
        public double GrossWeight
        {
            get { return _GrossWeight; }
            set
            {
                _GrossWeight = value;
                OnPropertyChanged("GrossWeight");
                UpdateNettWeight();
            }
        }

        private double _NettWeight;
        public double NettWeight
        {
            get { return _NettWeight; }
            set
            {
                _NettWeight = value;
                OnPropertyChanged("NettWeight");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the NewTicketViewModel class.
        /// </summary>
        public EditTicketViewModel()
            : base($"Edit Ticket")
        {
            // _ticket = ticket;
            ContentID = ToolContentID;
            LoadDropdowns();
        }

        #endregion

        #region Commands

        private ICommand _UpdateTicketCommand;
        /// <summary>
        /// </summary>
        public ICommand UpdateTicketCommand
        {
            get
            {
                if (_UpdateTicketCommand == null)
                {
                    _UpdateTicketCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                UpdateTicket();
                            }
                            catch (Exception ex)
                            {
                                MainViewModel.This.ShowExceptionMessageBox(ex);
                            }
                        });
                }

                return _UpdateTicketCommand;
            }
        }

        #endregion

        #region Private Methods

        private void LoadDropdowns()
        {

        }

        private void UpdateNettWeight()
        {
            NettWeight = GrossWeight - TareWeight;
        }

        private void UpdateTicket()
        {
            _ticket.LastModifiedBy = "Werner";
            _ticket.LastModified = DateTime.Now;
            _ticket.Status = "In progress";

            messageBoxService.ShowMessageBox($"Successfully updated ticket {_ticket.TicketNumber}", "Success", MessageBoxButton.OK);

            MainViewModel.This.StatusMessage = $"Successfully update ticket {_ticket.TicketNumber}";
            MainViewModel.This.TicketList.ReloadTickets();
        }

        #endregion
    }
}
