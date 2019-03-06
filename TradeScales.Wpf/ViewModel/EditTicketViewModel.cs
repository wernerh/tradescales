
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
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
    public class EditTicketViewModel : DocumentViewModel
    {

        #region Fields

        public const string ToolContentID = "EditTicketViewModel";

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
        public EditTicketViewModel(TicketViewModel ticket)
            : base($"Edit Ticket ({ticket.TicketNumber})")
        {
            EditTicket = Mapper.Map<Ticket, TicketViewModel>(_ticketsRepository.GetSingle(ticket.ID));
            ContentID = ToolContentID;
            InitialiseEditTicket();
        }

        #endregion

        #region Properties

        private TicketViewModel _EditTicket;
        public TicketViewModel EditTicket
        {
            get
            {
                if (_EditTicket == null)
                {
                    _EditTicket = new TicketViewModel();
                }
                return _EditTicket;
            }
            set
            {
                _EditTicket = value;
                OnPropertyChanged("EditTicket");
            }
        }

        public string TicketNumber
        {
            get { return EditTicket.TicketNumber; }
            set
            {
                EditTicket.TicketNumber = value;
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

                if(_SelectedHaulier != null)
                {
                    EditTicket.HaulierId = _SelectedHaulier.ID;
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
                    EditTicket.CustomerId = _SelectedCustomer.ID;
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
                    EditTicket.DestinationId = _SelectedDestination.ID;
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
                    EditTicket.ProductId = _SelectedProduct.ID;
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
                    EditTicket.DriverId = _SelectedDriver.ID;
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
                    EditTicket.VehicleId = _SelectedVehicle.ID;
                }
            }
        }

        public string OrderNumber
        {
            get { return EditTicket.OrderNumber; }
            set
            {
                EditTicket.OrderNumber = value;
                OnPropertyChanged("OrderNumber");
            }
        }

        public string DeliveryNumber
        {
            get { return EditTicket.DeliveryNumber; }
            set
            {
                EditTicket.DeliveryNumber = value;
                OnPropertyChanged("DeliveryNumber");
            }
        }

        public string SealTo
        {
            get { return EditTicket.SealTo; }
            set
            {
                EditTicket.SealTo = value;
                OnPropertyChanged("SealTo");
            }
        }

        public string SealFrom
        {
            get { return EditTicket.SealFrom; }
            set
            {
                EditTicket.SealFrom = value;
                OnPropertyChanged("SealFrom");
            }
        }

        public double TareWeight
        {
            get { return EditTicket.TareWeight; }
            set
            {
                EditTicket.TareWeight = value;
                OnPropertyChanged("TareWeight");
                UpdateNettWeight();
            }
        }

        public double GrossWeight
        {
            get { return EditTicket.GrossWeight; }
            set
            {
                EditTicket.GrossWeight = value;
                OnPropertyChanged("GrossWeight");
                UpdateNettWeight();
            }
        }

        public double NettWeight
        {
            get { return EditTicket.NettWeight; }
            set
            {
                EditTicket.NettWeight = value;
                OnPropertyChanged("NettWeight");
            }
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

        private void InitialiseEditTicket()
        {
            LoadDropdowns();
            SetValues();
        }

        private void LoadDropdowns()
        {
            Hauliers = Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(_hauliersRepository.GetAll());
            Customers = Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(_customersRepository.GetAll());
            Products = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productsRepository.GetAll());
            Destinations = Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(_destinationsRepository.GetAll());
            Drivers = Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(_driversRepository.GetAll()).OrderBy(d => d.FirstName);
            Vehicles = Mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleViewModel>>(_vehiclesRepository.GetAll()).OrderBy(d => d.Registration);
        }

        private void SetValues()
        {
            TimeIn = EditTicket.TimeIn.ToString();
            TimeOut = DateTime.Now.ToString();

            EditTicket.LastModifiedBy = MainViewModel.This.LoggedInUserContext.User.Username;
            EditTicket.LastModified = DateTime.Now;
            EditTicket.Status = "Modified";

            SelectedHaulier = Hauliers.FirstOrDefault(x => x.ID == EditTicket.HaulierId);
            SelectedCustomer = Customers.FirstOrDefault(x => x.ID == EditTicket.CustomerId);
            SelectedProduct = Products.FirstOrDefault(x => x.ID == EditTicket.ProductId);
            SelectedDestination = Destinations.FirstOrDefault(x => x.ID == EditTicket.DestinationId);
            SelectedDriver = Drivers.FirstOrDefault(x => x.ID == EditTicket.DriverId);
            SelectedVehicle = Vehicles.FirstOrDefault(x => x.ID == EditTicket.VehicleId);
        }

        private void UpdateNettWeight()
        {
            NettWeight = GrossWeight - TareWeight;
        }

        private void UpdateTicket()
        {
            Ticket ticket = _ticketsRepository.GetSingle(EditTicket.ID);
            ticket.UpdateTicket(EditTicket);
            _unitOfWork.Commit();
         
            messageBoxService.ShowMessageBox($"Successfully updated ticket {EditTicket.TicketNumber}", "Success", MessageBoxButton.OK);
            MainViewModel.This.StatusMessage = $"Successfully updated ticket {EditTicket.TicketNumber}";
            MainViewModel.This.TicketList.ReloadTickets();
        }

        #endregion
    }
}
