using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Properties;
using TradeScales.Wpf.Resources.Services.Interfaces;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel
{
    /// <summary>
    /// tickets viewmodel
    /// </summary>
    public class TicketListViewModel : DocumentViewModel
    {

        #region Fields

        public const string ToolContentID = "TicketList";

        private static IMessageBoxService _messageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Ticket> _ticketsRepository = BootStrapper.Resolve<IEntityBaseRepository<Ticket>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the StartPageViewModel class.
        /// </summary>
        public TicketListViewModel()
            : base("Ticket List")
        {
            ContentID = ToolContentID;
            ReloadTickets();
        }

        #endregion

        #region Properties

        private ObservableCollection<TicketViewModel> _Tickets;
        public ObservableCollection<TicketViewModel> Tickets
        {
            get { return _Tickets; }
            set
            {
                _Tickets = value;
                OnPropertyChanged("Tickets");
            }
        }

        #endregion

        #region Commands


        private ICommand _EditCommand;
        /// <summary>
        /// </summary>
        public ICommand EditCommand
        {
            get
            {
                if (_EditCommand == null)
                {
                    _EditCommand = new RelayCommand<TicketViewModel>(EditTicket);
                }
                return _EditCommand;
            }
        }

        private ICommand _ViewCommand;
        /// <summary>
        /// </summary>
        public ICommand ViewCommand
        {
            get
            {
                if (_ViewCommand == null)
                {
                    _ViewCommand = new RelayCommand<TicketViewModel>(ViewTicket);
                }
                return _ViewCommand;
            }
        }


        private ICommand _DeleteCommand;
        /// <summary>
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                {
                    _DeleteCommand = new RelayCommand<TicketViewModel>(DeleteTicket);
                }
                return _DeleteCommand;
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
                                ReloadTickets();
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

        public void ReloadTickets()
        {
            Tickets = new ObservableCollection<TicketViewModel>(Mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(_ticketsRepository.GetAll().OrderByDescending(t => t.TicketNumber)));
        }

        public void FilterTickets(DateTime dateFrom, DateTime dateTo, int haulierId, int customerId, int destinationId, int productId, int driverId, int vehicleId)
        {
            // Get All Tickets
            var tickets = _ticketsRepository.GetAll();

            //Filter tickets
            tickets = tickets.ToList().Where(t => DateTime.Parse(t.TimeIn) >= dateFrom).AsQueryable();
            tickets = tickets.ToList().Where(t => DateTime.Parse(t.TimeOut) <= dateTo).AsQueryable();

            // Haulier
            if (haulierId != -1)
            {
                tickets = tickets.Where(t => t.HaulierId == haulierId);
            }

            // Customer
            if (customerId != -1)
            {
                tickets = tickets.Where(t => t.CustomerId == customerId);
            }

            // Destination
            if (destinationId != -1)
            {
                tickets = tickets.Where(t => t.DestinationId == destinationId);
            }

            // Product
            if (productId != -1)
            {
                tickets = tickets.Where(t => t.ProductId == productId);
            }

            // Driver
            if (driverId != -1)
            {
                tickets = tickets.Where(t => t.DriverId == driverId);
            }

            // Vehicle
            if (vehicleId != -1)
            {
                tickets = tickets.Where(t => t.VehicleId == vehicleId);
            }

            Tickets = new ObservableCollection<TicketViewModel>(Mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(tickets.OrderByDescending(t => t.TicketNumber)));
        }

        #endregion

        #region Private Methods

        private void EditTicket(TicketViewModel ticket)
        {
            try
            {
                MainViewModel.This.OpenEditTicket(ticket);
            }
            catch (Exception ex)
            {
                MainViewModel.This.ShowExceptionMessageBox(ex);
            }
        }

        private void ViewTicket(TicketViewModel selectedTicket)
        {
            try
            {
                var ticket = _ticketsRepository.GetSingle(selectedTicket.ID);
                var rootPath = Settings.Default.WeighBridgeCertificatesFolder;
                var filePath = $"{rootPath}\\weighbridgecertificate.pdf";

                int copyNumber = 0;

                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                while (File.Exists(filePath))
                {
                    filePath = $"{rootPath}\\weighbridgecertificate - ({++copyNumber}).pdf";
                }

                ticket.Status = "Complete";
                _unitOfWork.Commit();

                GenerateTicket(filePath, ticket);
                MainViewModel.This.OpenPdfDocument(filePath);
                ReloadTickets();
            }
            catch (Exception ex)
            {
                MainViewModel.This.ShowExceptionMessageBox(ex);
            }
        }

        private void DeleteTicket(TicketViewModel ticket)
        {
            try
            {
                var ticketToDelete = _ticketsRepository.GetSingle(ticket.ID);
                var result = _messageBoxService.ShowMessageBox($"Are you sure you want to delte ticket {ticketToDelete.TicketNumber} ?", "Delete Ticket", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _ticketsRepository.Delete(ticketToDelete);
                    _unitOfWork.Commit();
                    _messageBoxService.ShowMessageBox($"successfully deleted ticket {ticketToDelete.TicketNumber}", "Success", MessageBoxButton.OK);
                    ReloadTickets();
                }
            }
            catch (Exception ex)
            {
                MainViewModel.This.ShowExceptionMessageBox(ex);
            }
        }

        private void GenerateTicket(string filepath, Ticket ticket)
        {
            // Create document
            Document document = new Document(PageSize.A4);
            var output = new FileStream(filepath, FileMode.Create);
            var writer = PdfWriter.GetInstance(document, output);
            document.Open();

            // Get logo path
            var logoPath = Settings.Default.WeighBridgeCertificateLogo;

            // Get Template path
            var templatePath = Settings.Default.WeighBridgeCertificateTemplate;

            // Read in the contents of the Receipt.htm file...
            string contents = File.ReadAllText(templatePath);

            // Replace the placeholders with the user-specified text
            contents = contents.Replace("[IMAGESOURCE]", logoPath);
            contents = contents.Replace("[TICKETNUMBER]", ticket.TicketNumber);
            contents = contents.Replace("[ORDERNUMBER]", ticket.OrderNumber);
            contents = contents.Replace("[DELIVERYNUMBER]", ticket.DeliveryNumber);
            contents = contents.Replace("[TIMEIN]", ticket.TimeIn.ToString());
            contents = contents.Replace("[TIMEOUT]", ticket.TimeOut.ToString());

            contents = contents.Replace("[CUSTOMERCODE]", ticket.Customer.Code);
            contents = contents.Replace("[CUSTOMERNAME]", ticket.Customer.Name);
            contents = contents.Replace("[HAULIERCODE]", ticket.Haulier.Code);
            contents = contents.Replace("[HAULIERNAME]", ticket.Haulier.Name);
            contents = contents.Replace("[DRIVERCODE]", ticket.Driver.Code);
            contents = contents.Replace("[DRIVERFIRSTNAME]", ticket.Driver.FirstName);
            contents = contents.Replace("[DRIVERLASTNAME]", ticket.Driver.LastName);
            contents = contents.Replace("[VEHICLECODE]", ticket.Vehicle.Code);
            contents = contents.Replace("[VEHICLEREGISTRATION]", ticket.Vehicle.Registration);
            contents = contents.Replace("[DESTINATIONCODE]", ticket.Destination.Code);
            contents = contents.Replace("[DESTINATIONNAME]", ticket.Destination.Name);
            contents = contents.Replace("[PODUCTCODE]", ticket.Product.Code);
            contents = contents.Replace("[PRODUCTNAME]", ticket.Product.Name);

            contents = contents.Replace("[TARREWEIGHT]", $"{ticket.TareWeight} kg");
            contents = contents.Replace("[GROSSWEIGHT]", $"{ticket.GrossWeight} kg");
            contents = contents.Replace("[NETTWEIGHT]", $"{ticket.NettWeight} kg");

            StringReader sr = new StringReader(contents);

            // Parse the HTML string into a collection of elements...
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);

            // Dispose resources
            document.Close();
        }
        #endregion

    }
}
