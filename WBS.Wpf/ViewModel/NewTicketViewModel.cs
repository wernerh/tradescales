
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WBS.Data.Extensions;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Entities;
using WBS.Wpf.Infrastructure.Extensions;
using WBS.Wpf.Model;
using WBS.Wpf.Properties;
using WBS.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = WBS.Wpf.Model.RelayCommand;

namespace WBS.Wpf.ViewModel
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
        private IEntityBaseRepository<Vehicle> _vehiclesRepository = BootStrapper.Resolve<IEntityBaseRepository<Vehicle>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

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

        private IEnumerable<HaulierViewModel> _Hauliers;
        public IEnumerable<HaulierViewModel> Hauliers
        {
            get { return _Hauliers; }
            set
            {
                _Hauliers = value;
                OnPropertyChanged("Hauliers");
            }
        }

        private HaulierViewModel _SelectedHaulier;
        public HaulierViewModel SelectedHaulier
        {
            get { return _SelectedHaulier; }
            set
            {
                _SelectedHaulier = value;
                OnPropertyChanged("SelectedHaulier");

                if (_SelectedHaulier != null)
                {
                    NewTicket.HaulierId = _SelectedHaulier.ID;
                }
            }
        }

        private IEnumerable<CustomerViewModel> _Customers;
        public IEnumerable<CustomerViewModel> Customers
        {
            get { return _Customers; }
            set
            {
                _Customers = value;
                OnPropertyChanged("Customers");
            }
        }

        private CustomerViewModel _SelectedCustomer;
        public CustomerViewModel SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");

                if (_SelectedCustomer != null)
                {
                    NewTicket.CustomerId = _SelectedCustomer.ID;
                }
            }
        }

        private IEnumerable<DestinationViewModel> _Destinations;
        public IEnumerable<DestinationViewModel> Destinations
        {
            get { return _Destinations; }
            set
            {
                _Destinations = value;
                OnPropertyChanged("Destinations");
            }
        }

        private DestinationViewModel _SelectedDestination;
        public DestinationViewModel SelectedDestination
        {
            get { return _SelectedDestination; }
            set
            {
                _SelectedDestination = value;
                OnPropertyChanged("SelectedDestination");

                if (_SelectedDestination != null)
                {
                    NewTicket.DestinationId = _SelectedDestination.ID;
                }
            }
        }

        private IEnumerable<ProductViewModel> _Products;
        public IEnumerable<ProductViewModel> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged("Products");
            }
        }

        private ProductViewModel _SelectedProduct;
        public ProductViewModel SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged("SelectedProduct");

                if (_SelectedProduct != null)
                {
                    NewTicket.ProductId = _SelectedProduct.ID;
                }
            }
        }

        private IEnumerable<DriverViewModel> _Drivers;
        public IEnumerable<DriverViewModel> Drivers
        {
            get { return _Drivers; }
            set
            {
                _Drivers = value;
                OnPropertyChanged("Drivers");
            }
        }

        private DriverViewModel _SelectedDriver;
        public DriverViewModel SelectedDriver
        {
            get { return _SelectedDriver; }
            set
            {
                _SelectedDriver = value;
                OnPropertyChanged("SelectedDriver");

                if (_SelectedDriver != null)
                {
                    NewTicket.DriverId = _SelectedDriver.ID;
                }
            }
        }

        private IEnumerable<VehicleViewModel> _Vehicles;
        public IEnumerable<VehicleViewModel> Vehicles
        {
            get { return _Vehicles; }
            set
            {
                _Vehicles = value;
                OnPropertyChanged("Vehicles");
            }
        }

        private VehicleViewModel _SelectedVehicle;
        public VehicleViewModel SelectedVehicle
        {
            get { return _SelectedVehicle; }
            set
            {
                _SelectedVehicle = value;
                OnPropertyChanged("SelectedVehicle");

                if (_SelectedVehicle != null)
                {
                    NewTicket.VehicleId = _SelectedVehicle.ID;
                }
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
                UpdateNetWeight();
            }
        }

        public double GrossWeight
        {
            get { return NewTicket.GrossWeight; }
            set
            {
                NewTicket.GrossWeight = value;
                OnPropertyChanged("GrossWeight");
                UpdateNetWeight();
            }
        }

        public double NetWeight
        {
            get { return NewTicket.NettWeight; }
            set
            {
                NewTicket.NettWeight = value;
                OnPropertyChanged("NetWeight");
            }
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
                    _CreateNewTicketCommand = new MVVMRelayCommand(execute => CreateNewTicket(), canExecute => CanCreateNewTicket());
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

        public override void ReloadEntities()
        {
            // Cache selected values
            var selectedHaulier = SelectedHaulier;
            var selectedCustomer = SelectedCustomer;
            var selectedDestination = SelectedDestination;
            var selectedProduct = SelectedProduct;
            var selectedDriver = SelectedDriver;
            var selectedVehicle = SelectedVehicle;

            // Reload dropdowns
            LoadDropdowns();

            // Set selected values
            SelectedHaulier = selectedHaulier;
            SelectedCustomer = selectedCustomer;
            SelectedDestination = selectedDestination;
            SelectedProduct = selectedProduct;
            SelectedDriver = selectedDriver;
            SelectedVehicle = selectedVehicle;
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
            Hauliers = Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(_hauliersRepository.GetAll().OrderBy(x => x.Name));
            Customers = Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(_customersRepository.GetAll().OrderBy(x => x.Name));
            Products = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productsRepository.GetAll().OrderBy(x => x.Name));
            Destinations = Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(_destinationsRepository.GetAll().OrderBy(x => x.Name));
            Drivers = Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(_driversRepository.GetAll()).OrderBy(d => d.FirstName);
            Vehicles = Mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleViewModel>>(_vehiclesRepository.GetAll()).OrderBy(d => d.Registration);
        }

        private void LoadDefaultValues()
        {
            NewTicket = null;

            var lastTicketNumber = Settings.Default.LastTicketNumber;
            var numberOfTickets = int.Parse(lastTicketNumber);
            var newTicketNumber = (++numberOfTickets).ToString("D6");

            Settings.Default.LastTicketNumber = newTicketNumber;
            Settings.Default.Save();

            SelectedHaulier = Hauliers.FirstOrDefault();
            SelectedCustomer = Customers.FirstOrDefault();
            SelectedProduct = Products.FirstOrDefault();
            SelectedDestination = Destinations.FirstOrDefault();
            SelectedDriver = Drivers.FirstOrDefault();
            SelectedVehicle = Vehicles.FirstOrDefault();

            TicketNumber = $"#{newTicketNumber}";
            OrderNumber = newTicketNumber;
            DeliveryNumber = newTicketNumber;
            SealFrom = $"Seal From ({TicketNumber})";
            SealTo = $"Seal To ({TicketNumber})";
            TimeIn = DateTime.Now.ToString();
            TareWeight = 0;
            GrossWeight = 0;
            NetWeight = 0;

            NewTicket.TimeIn = DateTime.Parse(TimeIn);
            NewTicket.TimeOut = DateTime.Now;
            NewTicket.LastModified = DateTime.Now;
            NewTicket.Status = "In Progress";
        }

        private void UpdateNetWeight()
        {
            NetWeight = GrossWeight - TareWeight;
        }

        private bool CanCreateNewTicket()
        {
            return _hauliersRepository.GetAll().Any()
                && _customersRepository.GetAll().Any()
                && _productsRepository.GetAll().Any()
                && _destinationsRepository.GetAll().Any()
                && _driversRepository.GetAll().Any()
                && _vehiclesRepository.GetAll().Any();
        }

        private void CreateNewTicket()
        {
            try
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
            catch (Exception exception)
            {
                MainViewModel.This.ShowExceptionMessageBox(exception);
            }
        }
        #endregion
    }
}
