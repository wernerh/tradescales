
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TradeScales.Wpf.ViewModel
{
    /// <summary>
    /// New ticket view model
    /// </summary>
    public class NewTicketViewModel : DocumentViewModel
    {

        #region Fields

        public const string ToolContentID = "NewTicketViewModel";

        private static IMessageBoxService messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Ticket> _ticketsRepository = BootStrapper.Resolve<IEntityBaseRepository<Ticket>>();
        private IEntityBaseRepository<Haulier> _hauliersRepository = BootStrapper.Resolve<IEntityBaseRepository<Haulier>>();
        private IEntityBaseRepository<Customer> _customersRepository = BootStrapper.Resolve<IEntityBaseRepository<Customer>>();
        private IEntityBaseRepository<Product> _productsRepository = BootStrapper.Resolve<IEntityBaseRepository<Product>>();
        private IEntityBaseRepository<Destination> _destinationsRepository = BootStrapper.Resolve<IEntityBaseRepository<Destination>>();
        private IEntityBaseRepository<Driver> _driversRepository = BootStrapper.Resolve<IEntityBaseRepository<Driver>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Properties

        private TicketViewModel _NewTicket;
        public TicketViewModel NewTicket
        {
            get
            {
                if (_NewTicket == null)
                {
                    _NewTicket = new TicketViewModel();
                }
                return _NewTicket;
            }
            set
            {
                _NewTicket = value;
                OnPropertyChanged("NewTicket");
            }
        }

        public string TicketNumber
        {
            get { return NewTicket.TicketNumber; }
            set
            {
                NewTicket.TicketNumber = value;
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

        public IEnumerable<HaulierViewModel> Hauliers { get; set; }

        private HaulierViewModel _SelectedHaulier;
        public HaulierViewModel SelectedHaulier
        {
            get { return _SelectedHaulier; }
            set
            {
                _SelectedHaulier = value;
                NewTicket.HaulierId = _SelectedHaulier.ID;
                OnPropertyChanged("SelectedHaulier");
            }
        }

        public IEnumerable<CustomerViewModel> Customers { get; set; }

        private CustomerViewModel _SelectedCustomer;
        public CustomerViewModel SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                NewTicket.CustomerId = _SelectedCustomer.ID;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        public IEnumerable<DestinationViewModel> Destinations { get; set; }

        private DestinationViewModel _SelectedDestination;
        public DestinationViewModel SelectedDestination
        {
            get { return _SelectedDestination; }
            set
            {
                _SelectedDestination = value;
                NewTicket.DestinationId = _SelectedDestination.ID;
                OnPropertyChanged("SelectedDestination");
            }
        }

        public IEnumerable<ProductViewModel> Products { get; set; }

        private ProductViewModel _SelectedProduct;
        public ProductViewModel SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                NewTicket.ProductId = _SelectedProduct.ID;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public IEnumerable<DriverViewModel> Drivers { get; set; }

        private DriverViewModel _SelectedDriver;
        public DriverViewModel SelectedDriver
        {
            get { return _SelectedDriver; }
            set
            {
                _SelectedDriver = value;
                NewTicket.DriverId = _SelectedDriver.ID;
                OnPropertyChanged("SelectedDriver");
            }
        }

        public string OrderNumber
        {
            get { return NewTicket.OrderNumber; }
            set
            {
                NewTicket.OrderNumber = value;
                OnPropertyChanged("OrderNumber");
            }
        }

        public string DeliveryNumber
        {
            get { return NewTicket.DeliveryNumber; }
            set
            {
                NewTicket.DeliveryNumber = value;
                OnPropertyChanged("DeliveryNumber");
            }
        }

        public string SealTo
        {
            get { return NewTicket.SealTo; }
            set
            {
                NewTicket.SealTo = value;
                OnPropertyChanged("SealTo");
            }
        }

        public string SealFrom
        {
            get { return NewTicket.SealFrom; }
            set
            {
                NewTicket.SealFrom = value;
                OnPropertyChanged("SealFrom");
            }
        }

        public double TareWeight
        {
            get { return NewTicket.TareWeight; }
            set
            {
                NewTicket.TareWeight = value;
                OnPropertyChanged("TareWeight");
                UpdateNettWeight();
            }
        }

        public double GrossWeight
        {
            get { return NewTicket.GrossWeight; }
            set
            {
                NewTicket.GrossWeight = value;
                OnPropertyChanged("GrossWeight");
                UpdateNettWeight();
            }
        }

        public double NettWeight
        {
            get { return NewTicket.NettWeight; }
            set
            {
                NewTicket.NettWeight = value;
                OnPropertyChanged("NettWeight");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the NewTicketViewModel class.
        /// </summary>
        public NewTicketViewModel()
            : base("New Ticket")
        {
            ContentID = ToolContentID;
            InitialiseNewTicket();
        }

        #endregion

        #region Commands

        private ICommand _CreateNewTicketCommand;
        /// <summary>
        /// </summary>
        public ICommand CreateNewTicketCommand
        {
            get
            {
                if (_CreateNewTicketCommand == null)
                {
                    _CreateNewTicketCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                CreateNewTicket();
                            }
                            catch (Exception ex)
                            {
                                MainViewModel.This.ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _CreateNewTicketCommand;
            }
        }

        #endregion

        #region Public Methods

        public override void UpdateWeight(double weightReading, bool isReceiving)
        {
            if (isReceiving)
            {
                TareWeight = weightReading;
            }
            else
            {
                GrossWeight = weightReading;
            }
        }

        #endregion

        #region Private Methods

        private void InitialiseNewTicket()
        {
            LoadDropdowns();
            LoadDefaultValues();
        }

        private void LoadDropdowns()
        {
            Hauliers = Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(_hauliersRepository.GetAll());
            Customers = Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(_customersRepository.GetAll());
            Products = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productsRepository.GetAll());
            Destinations = Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(_destinationsRepository.GetAll());
            Drivers = Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(_driversRepository.GetAll()).OrderBy(d => d.FirstName);
        }

        private void LoadDefaultValues()
        {
            NewTicket = null;
        
            var lastTicketNumber = _ticketsRepository.GetAll().OrderByDescending(t => t.TicketNumber).FirstOrDefault()?.TicketNumber.Replace("#", "") ?? "000000";
            var numberOfTickets = int.Parse(lastTicketNumber);
            var newTicketNumber = (++numberOfTickets).ToString("D6");

            SelectedHaulier = Hauliers.First();
            SelectedCustomer = Customers.First();
            SelectedProduct = Products.First();
            SelectedDestination = Destinations.First();
            SelectedDriver = Drivers.First();

            TicketNumber = $"#{newTicketNumber}";
            OrderNumber = newTicketNumber;
            DeliveryNumber = newTicketNumber;
            SealFrom = $"Seal From ({TicketNumber})";
            SealTo = $"Seal To ({TicketNumber})";
            TimeIn = DateTime.Now.ToString();
            TareWeight = 0;
            GrossWeight = 0;
            NettWeight = 0;

            NewTicket.TimeIn = DateTime.Parse(TimeIn);
            NewTicket.TimeOut = DateTime.Now;
            NewTicket.LastModified = DateTime.Now;
            NewTicket.Status = "In Progress";
        }

        private void UpdateNettWeight()
        {
            NettWeight = GrossWeight - TareWeight;
        }

        private void CreateNewTicket()
        {
            if (_ticketsRepository.TicketExists(NewTicket.TicketNumber))
            {
                throw new ArgumentException("Ticket already exists");
            }
            else
            {
                NewTicket.LastModifiedBy = MainViewModel.This.LoggedInUserContext.User.Username;

                Ticket ticket = new Ticket();
                ticket.UpdateTicket(NewTicket);

                _ticketsRepository.Add(ticket);
                _unitOfWork.Commit();

                messageBoxService.ShowMessageBox($"successfully added new ticket {NewTicket.TicketNumber}", "Success", MessageBoxButton.OK);
                MainViewModel.This.StatusMessage = $"successfully added new ticket {NewTicket.TicketNumber}";
                MainViewModel.This.TicketList.ReloadTickets();
                LoadDefaultValues();
            }
        }
        #endregion
    }
}
