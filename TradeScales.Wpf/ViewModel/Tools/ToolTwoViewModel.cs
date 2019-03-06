using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Properties;
using TradeScales.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Tools
{
    /// <summary>
    /// Tool two viewmodel
    /// </summary>
    public class ToolTwoViewModel : ToolViewModel
    {

        #region Fields

        public const string ToolContentID = "ToolTwo";

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

        /// <summary>
        /// Initializes a new instance of the ToolTwoViewModel class
        /// </summary>
        public ToolTwoViewModel()
            : base("Reports")
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

        private ICommand _GenerateReportCommand;
        /// <summary>
        /// </summary>
        public ICommand GenerateReportCommand
        {
            get
            {
                if (_GenerateReportCommand == null)
                {
                    _GenerateReportCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                ViewReport();
                            }
                            catch (Exception ex)
                            {
                                MainViewModel.This.ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _GenerateReportCommand;
            }
        }

        private ICommand _RefreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_RefreshCommand == null)
                {
                    _RefreshCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                throw (new Exception("Refresh"));

                            }
                            catch (Exception ex)
                            {
                                MainViewModel.This.ShowExceptionMessageBox(ex);
                            }
                        });
                }

                return _RefreshCommand;
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
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now.AddDays(1);

            SelectedHaulier = Hauliers.FirstOrDefault();
            SelectedCustomer = Customers.FirstOrDefault();
            SelectedProduct = Products.FirstOrDefault();
            SelectedDestination = Destinations.FirstOrDefault();
            SelectedDriver = Drivers.FirstOrDefault();
            SelectedVehicle = Vehicles.FirstOrDefault();
        }

        private void ViewReport()
        {
            string rootPath = Settings.Default.ReportsFolder;
            var filePath = $"{rootPath}\\Report.pdf";

            // Get Report Data
            var tickets = GetReportData();

            if (tickets.Count() == 0)
            {
                _MessageBoxService.ShowMessageBox("No tickets found. Please adjust your filters.", "Report Generation", MessageBoxButton.OK);
                return;
            }

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            int copyNumber = 0;

            while (File.Exists(filePath))
            {
                filePath = $"{rootPath}\\Report - ({++copyNumber}).pdf";
            }

            GenerateReport(filePath, tickets);
            MainViewModel.This.OpenPdfDocument(filePath);
        }

        private void GenerateReport(string filePath, IEnumerable<Ticket> tickets)
        {

            // Create document
            Document document = new Document(PageSize.A4.Rotate());
            var output = new FileStream(filePath, FileMode.Create);
            var writer = PdfWriter.GetInstance(document, output);
            document.Open();

            // Get logo path
            var logoPath = Settings.Default.ReportLogo;

            // Get template path
            var templatePath = Settings.Default.ReportTemplate;

            // Read in the contents of the Receipt.htm file...
            string contents = File.ReadAllText(templatePath);

            // Replace the placeholders with the user-specified text
            contents = contents.Replace("[IMAGESOURCE]", logoPath);
            contents = contents.Replace("[TIMESTAMP]", $"{DateFrom} - {DateTo}");
            contents = contents.Replace("[TICKETS]", GetTicketTable(tickets));

            StringReader sr = new StringReader(contents);

            // Parse the HTML string into a collection of elements...
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);

            // Dispose resources
            document.Close();
        }

        private IEnumerable<Ticket> GetReportData()
        {
            // Get All Tickets
            var tickets = _ticketsRepository.GetAll();

            //Filter tickets
            tickets = tickets.ToList().Where(t => DateTime.Parse(t.TimeIn) >= DateFrom).AsQueryable();
            tickets = tickets.ToList().Where(t => DateTime.Parse(t.TimeOut) <= DateTo).AsQueryable();

            // Haulier
            if (SelectedHaulier.ID != -1)
            {
                tickets = tickets.Where(t => t.HaulierId == SelectedHaulier.ID);
            }

            // Customer
            if (SelectedCustomer.ID != -1)
            {
                tickets = tickets.Where(t => t.CustomerId == SelectedCustomer.ID);
            }

            // Destination
            if (SelectedDestination.ID != -1)
            {
                tickets = tickets.Where(t => t.DestinationId == SelectedDestination.ID);
            }

            // Product
            if (SelectedProduct.ID != -1)
            {
                tickets = tickets.Where(t => t.ProductId == SelectedProduct.ID);
            }

            // Driver
            if (SelectedDriver.ID != -1)
            {
                tickets = tickets.Where(t => t.DriverId == SelectedDriver.ID);
            }

            // Vehicle
            if (SelectedVehicle.ID != -1)
            {
                tickets = tickets.Where(t => t.VehicleId == SelectedDriver.ID);
            }

            return tickets;
        }

        private string GetTicketTable(IEnumerable<Ticket> tickets)
        {
            var result = @"<table><tr><th style=""font-weight: bold"">Ticket No</th><th style=""font-weight: bold"">Date Out</th><th style=""font-weight: bold"">Time Out</th><th style=""font-weight: bold"">Driver</th><th style=""font-weight: bold"">Registration</th><th style=""font-weight: bold"">Order No</th><th style=""font-weight: bold"">Delivery No</th><th style=""font-weight: bold"">Gross</th><th style=""font-weight: bold"">Tare</th><th style=""font-weight: bold"">Nett</th></tr>";

            foreach (var ticket in tickets)
            {
                result += $"<tr><td>{ticket.TicketNumber}</td><td>{DateTime.Parse(ticket.TimeOut).ToShortDateString()}</td><td>{DateTime.Parse(ticket.TimeOut).ToShortTimeString()}</td><td>{ticket.Driver.FirstName} {ticket.Driver.LastName}</td><td>{ticket.Vehicle.Registration}</td><td>{ticket.OrderNumber}</td><td>{ticket.DeliveryNumber}</td><td>{ticket.GrossWeight}</td><td>{ticket.TareWeight}</td><td>{ticket.NettWeight}</td></tr>";
            }

            // Calculate Totals
            var totalGross = tickets.Sum(x => x.GrossWeight);
            var totalTare = tickets.Sum(x => x.TareWeight);
            var totalNett = tickets.Sum(x => x.NettWeight);

            result += $"<tr><td>Grand Total:</td><td></td><td></td><td></td><td></td><td></td><td></td><td>{totalGross}</td><td>{totalTare}</td><td>{totalNett}</td></tr>";
            result += "</table>";

            return result;
        }

        #endregion

    }
}
