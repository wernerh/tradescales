using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Wpf.Model;
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
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

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

        public IEnumerable<HaulierViewModel> Hauliers { get; set; }
      
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

        public IEnumerable<CustomerViewModel> Customers { get; set; }

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

        public IEnumerable<DestinationViewModel> Destinations { get; set; }

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

        public IEnumerable<ProductViewModel> Products { get; set; }

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

        public IEnumerable<DriverViewModel> Drivers { get; set; }

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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ToolTwoViewModel class
        /// </summary>
        public ToolTwoViewModel()
            : base("Reports")
        {
            ContentID = ToolContentID;
            LoadDropdowns();
            SetDefaultValues();
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

        #region Private Methods

        private void LoadDropdowns()
        {
            var hauliers = new List<HaulierViewModel>();
            hauliers.Add(new HaulierViewModel() { Name = "All", ID = -1 });
            hauliers.AddRange(Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(_hauliersRepository.GetAll()));
            Hauliers = hauliers;

   
            Hauliers = hauliers;

            var customers = new List<CustomerViewModel>();
            customers.Add(new CustomerViewModel() { Name = "All", ID = -1 });
            customers.AddRange(Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(_customersRepository.GetAll()));
            Customers = customers;

            var products = new List<ProductViewModel>();
            products.Add(new ProductViewModel() { Name = "All", ID = -1 });
            products.AddRange(Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productsRepository.GetAll()));
            Products = products;

            var destinations = new List<DestinationViewModel>();
            destinations.Add(new DestinationViewModel() { Name = "All", ID = -1 });
            destinations.AddRange(Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(_destinationsRepository.GetAll()));
            Destinations = destinations;

            var drivers = new List<DriverViewModel>();
            drivers.Add(new DriverViewModel() { FirstName = "All", ID = -1 });
            drivers.AddRange(Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(_driversRepository.GetAll()).OrderBy(d => d.FirstName));
            Drivers = drivers;
        }

        private void SetDefaultValues()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now.AddDays(1);

            SelectedHaulier = Hauliers.First();
            SelectedCustomer = Customers.First();
            SelectedProduct = Products.First();
            SelectedDestination = Destinations.First();
            SelectedDriver = Drivers.First();
        }

        private void ViewReport()
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{rootPath}\\Report.pdf";

            // Get Report Data
            var tickets = GetReportData();

            if (tickets.Count() == 0)
            {
                _MessageBoxService.ShowMessageBox("No tickets were generated in your selected time period.\r\nPlease adjust your 'Date To' or 'Date From' filter.", "Report Generation", MessageBoxButton.OK);
                return;
            }

            int copyNumber = 0;
            while(File.Exists(filePath))
            {
                filePath = $"{rootPath}\\Report - ({++copyNumber}).pdf";
            }

            GenerateReport(filePath, tickets);
            MainViewModel.This.OpenPdfDocument(filePath);
        }

        private void GenerateReport(string filePath, IEnumerable<Ticket> tickets)
        {
          
            // Get root path
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Create document
            Document document = new Document(PageSize.A4.Rotate());
            var output = new FileStream(filePath, FileMode.Create);       
            var writer = PdfWriter.GetInstance(document, output);
            document.Open();

            // Get logo path
            var logoPath = $"{rootPath}\\Resources\\images\\tradescales.png";

            // Read in the contents of the Receipt.htm file...
            string contents = File.ReadAllText($"{rootPath}\\Resources\\templates\\ReportTemplate.html");

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
            tickets = tickets.Where(t => t.TimeIn >= DateFrom);
            tickets = tickets.Where(t => t.TimeOut <= DateTo);

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

            return tickets;
        }

        private string GetTicketTable(IEnumerable<Ticket> tickets)
        {
            var result = @"<table><tr><th style=""font-weight: bold"">Ticket No</th><th style=""font-weight: bold"">Date Out</th><th style=""font-weight: bold"">Time Out</th><th style=""font-weight: bold"">Driver</th><th style=""font-weight: bold"">Order No</th><th style=""font-weight: bold"">Delivery No</th><th style=""font-weight: bold"">Gross</th><th style=""font-weight: bold"">Tare</th><th style=""font-weight: bold"">Nett</th></tr>";

            foreach (var ticket in tickets)
            {
                result += $"<tr><td>{ticket.TicketNumber}</td><td>{ticket.TimeOut.ToShortDateString()}</td><td>{ticket.TimeOut.ToShortTimeString()}</td><td>{ticket.Driver.FirstName} {ticket.Driver.LastName}</td><td>{ticket.OrderNumber}</td><td>{ticket.DeliveryNumber}</td><td>{ticket.GrossWeight}</td><td>{ticket.TareWeight}</td><td>{ticket.NettWeight}</td></tr>";
            }

            // Calculate Totals
            var totalGross = tickets.Sum(x => x.GrossWeight);
            var totalTare = tickets.Sum(x => x.TareWeight);
            var totalNett = tickets.Sum(x => x.NettWeight);

            result += $"<tr><td>Grand Total:</td><td></td><td></td><td></td><td></td><td></td><td>{totalGross}</td><td>{totalTare}</td><td>{totalNett}</td></tr>";
            result += "</table>";

            return result;
        }

        #endregion

    }
}
