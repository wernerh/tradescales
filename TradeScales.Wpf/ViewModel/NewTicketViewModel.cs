
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services.Interfaces;

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
        private static TradeScalesEntities context = new TradeScalesEntities();

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

        private ObservableCollection<Haulier> Hauliers { get; set; }

        private Haulier _SelectedHaulier;
        public Haulier SelectedHaulier
        {
            get { return _SelectedHaulier; }
            set
            {
                _SelectedHaulier = value;
                OnPropertyChanged("SelectedHaulier");
            }
        }

        public ObservableCollection<Customer> Customers { get; set; }

        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        public ObservableCollection<Destination> Destinations { get; set; }

        private Destination _SelectedDestination;
        public Destination SelectedDestination
        {
            get { return _SelectedDestination; }
            set
            {
                _SelectedDestination = value;
                OnPropertyChanged("SelectedDestination");
            }
        }

        public ObservableCollection<Product> Products { get; set; }

        private Product _SelectedProduct;
        public Product SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public ObservableCollection<Driver> Drivers { get; set; }

        private Driver _SelectedDriver;
        public Driver SelectedDriver
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
                    _CreateNewTicketCommand = new RelayCommand(canExecute => { CreateNewTicket(); });
                }
                return _CreateNewTicketCommand;
            }
        }

        #endregion

        #region Private Methods
        private void InitialiseNewTicket()
        {
            LoadDropdowns();
            LoadDefaultValues();       
        }

        private void LoadDefaultValues()
        {
            context.Tickets.Load();
            var numberOfTickets = context.Tickets.Local.Count;
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
        }

        private void LoadDropdowns()
        {
            context.Hauliers.Load();
            context.Customers.Load();
            context.Products.Load();
            context.Destinations.Load();
            context.Drivers.Load();

            Hauliers = context.Hauliers.Local;
            Customers = context.Customers.Local;
            Products = context.Products.Local;
            Destinations = context.Destinations.Local;
            Drivers = context.Drivers.Local;
        }

        private void UpdateNettWeight()
        {
            NettWeight = GrossWeight - TareWeight;
        }

        private void CreateNewTicket()
        {
            var ticket = new Ticket();

            ticket.LastModifiedBy = "Werner";
            ticket.Status = "New Ticket";
            ticket.TicketNumber = TicketNumber;
            ticket.TimeIn = DateTime.Parse(TimeIn);
            ticket.TimeOut = DateTime.UtcNow;
            ticket.LastModified = DateTime.Now;
            ticket.HaulierId = SelectedHaulier.ID;
            ticket.CustomerId = SelectedCustomer.ID;
            ticket.DestinationId = SelectedDestination.ID;
            ticket.ProductId = SelectedProduct.ID;
            ticket.DriverId = SelectedDriver.ID;
            ticket.OrderNumber = OrderNumber;
            ticket.DeliveryNumber = DeliveryNumber;
            ticket.SealFrom = SealFrom;
            ticket.SealTo = SealTo;
            ticket.TareWeight = TareWeight;
            ticket.GrossWeight = GrossWeight;
            ticket.NettWeight = NettWeight;

            context.Tickets.Add(ticket);
            context.SaveChanges();

            messageBoxService.ShowMessageBox($"successfully added new ticket {ticket.TicketNumber}", "Success", MessageBoxButton.OK);
            MainViewModel.This.StatusMessage = $"successfully added new ticket {ticket.TicketNumber}";
            MainViewModel.This.TicketList.LoadTickets();
            LoadDefaultValues();
        }

        #endregion
    }
}
