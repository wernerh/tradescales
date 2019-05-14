using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Entities;
using WBS.Wpf.Model;
using WBS.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = WBS.Wpf.Model.RelayCommand;

namespace WBS.Wpf.ViewModel.Tools
{

    public class ToolThreeViewModel : ToolViewModel
    {

        #region Fields

        public const string ToolContentID = "ToolThree";

        private static IMessageBoxService _MessageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

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

        public ToolThreeViewModel()
            : base("Ticket List Filter")
        {
            ContentID = ToolContentID;
            ReloadEntities();          
        }

        #endregion

        #region Properties

        private DateTime _DateFrom;
        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set
            {
                if (_DateFrom != value)
                {
                    _DateFrom = value;
                    OnPropertyChanged("DateFrom");
                }
            }
        }

        private DateTime _DateTo;
        public DateTime DateTo
        {
            get { return _DateTo; }
            set
            {
                if (_DateTo != value)
                {
                    _DateTo = value;
                    OnPropertyChanged("DateTo");
                }
            }
        }


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
            }
        }

        #endregion

        #region Commands

        private ICommand _FilterTicketListCommand;    
        public ICommand FilterTicketListCommand
        {
            get
            {
                if (_FilterTicketListCommand == null)
                {
                    _FilterTicketListCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                FilterTicketList();
                            }
                            catch (Exception ex)
                            {
                                MainViewModel.This.ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _FilterTicketListCommand;
            }
        }

        #endregion

        #region Public Methods

        public void ReloadEntities()
        {
            LoadDropdowns();
            SetDefaultValues();
        }

        #endregion

        #region Private Methods

        private void LoadDropdowns()
        {
            var hauliers = new List<HaulierViewModel>();
            hauliers.Add(new HaulierViewModel() { Name = "All", ID = -1 });
            hauliers.AddRange(Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(_hauliersRepository.GetAll().OrderBy(x => x.Name)));
            Hauliers = hauliers;
           
            var customers = new List<CustomerViewModel>();
            customers.Add(new CustomerViewModel() { Name = "All", ID = -1 });
            customers.AddRange(Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(_customersRepository.GetAll().OrderBy(x => x.Name)));
            Customers = customers;

            var products = new List<ProductViewModel>();
            products.Add(new ProductViewModel() { Name = "All", ID = -1 });
            products.AddRange(Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productsRepository.GetAll().OrderBy(x => x.Name)));
            Products = products;

            var destinations = new List<DestinationViewModel>();
            destinations.Add(new DestinationViewModel() { Name = "All", ID = -1 });
            destinations.AddRange(Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(_destinationsRepository.GetAll().OrderBy(x => x.Name)));
            Destinations = destinations;

            var drivers = new List<DriverViewModel>();
            drivers.Add(new DriverViewModel() { FirstName = "All", ID = -1 });
            drivers.AddRange(Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(_driversRepository.GetAll()).OrderBy(d => d.FirstName));
            Drivers = drivers;

            var vehicles = new List<VehicleViewModel>();
            vehicles.Add(new VehicleViewModel() { Registration = "All", ID = -1 });
            vehicles.AddRange(Mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleViewModel>>(_vehiclesRepository.GetAll()).OrderBy(x => x.Registration));
            Vehicles = vehicles;
        }

        private void SetDefaultValues()
        {
            DateFrom = DateTime.Now.AddHours(-1);
            DateTo = DateTime.Now.AddDays(1);

            SelectedHaulier = Hauliers.FirstOrDefault();
            SelectedCustomer = Customers.FirstOrDefault();
            SelectedProduct = Products.FirstOrDefault();
            SelectedDestination = Destinations.FirstOrDefault();
            SelectedDriver = Drivers.FirstOrDefault();
            SelectedVehicle = Vehicles.FirstOrDefault(); 
        }  

        private void FilterTicketList()
        {
            MainViewModel.This.TicketList.FilterTickets(DateFrom, DateTo, TicketNumber, SelectedHaulier.ID, SelectedCustomer.ID, SelectedDestination.ID, SelectedProduct.ID, SelectedDriver.ID, SelectedVehicle.ID);  
        }
     
        #endregion

    }
}
