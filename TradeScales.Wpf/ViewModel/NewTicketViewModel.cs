
using System;
using System.Collections.Generic;
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
    /// NewTicketViewModel
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
            }
        }

        private string _TimeIn;
        public string TimeIn
        {
            get { return _TimeIn; }
            set
            {
                _TimeIn = value;
            }
        }

        private string _TimeOut;
        public string TimeOut
        {
            get { return _TimeOut; }
            set
            {
                _TimeOut = value;
            }
        }


        private ObservableCollection<Haulier> _Hauliers;
        public ObservableCollection<Haulier> Hauliers
        {
            get { return _Hauliers; }
            set
            {
                _Hauliers = value;
                OnPropertyChanged("Hauliers");
            }
        }

        private Haulier _SelectedHaulier;
        public Haulier SelectedHaulier
        {
            get { return _SelectedHaulier; }
            set
            {
                _SelectedHaulier = value;
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
            }
        }

        private string _OrderNumber;
        public string OrderNumber
        {
            get { return _OrderNumber; }
            set
            {
                _OrderNumber = value;
            }
        }

        private string _DeliveryNumber;
        public string DeliveryNumber
        {
            get { return _DeliveryNumber; }
            set
            {
                _DeliveryNumber = value;
            }
        }

        private string _SealTo;
        public string SealTo
        {
            get { return _SealTo; }
            set
            {
                _SealTo = value;
            }
        }

        private string _SealFrom;
        public string SealFrom
        {
            get { return _SealFrom; }
            set
            {
                _SealFrom = value;
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
            LoadNewTicketNumber();
            LoadDropdowns();
        }

        private void LoadNewTicketNumber()
        {
            context.Tickets.Load();
            var numberOfTickets = context.Tickets.Local.Count;
            var newTicketNumber = (++numberOfTickets).ToString("D6");

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

            SelectedHaulier = Hauliers.First();
            SelectedCustomer = Customers.First();
            SelectedProduct = Products.First();
            SelectedDestination = Destinations.First();
            SelectedDriver = Drivers.First();
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
            LoadNewTicketNumber();
        }

        #endregion
    }
}
