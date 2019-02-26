﻿using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Timers;
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

        #region Commands

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
            Tickets = new ObservableCollection<TicketViewModel>(Mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(_ticketsRepository.GetAll()));
        }

        #endregion

        #region Private Methods

        private void ViewTicket(TicketViewModel selectedTicket)
        {
            try
            {
                var ticket = _ticketsRepository.GetSingle(selectedTicket.ID);
                var rootPath = Settings.Default.WeighBridgeCertificatesFolder;
                var filePath = $"{rootPath}\\weighbridgecertificate.pdf";

                int copyNumber = 0;

                if(!Directory.Exists(rootPath))
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

        private void EditTicket(TicketViewModel ticket)
        {
            MainViewModel.This.OpenEditTicket(ticket);
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
            contents = contents.Replace("[DRIVERMOBILE]", ticket.Driver.Mobile);
            contents = contents.Replace("[VEHICLEREGISTRATION]", ticket.Driver.VehicleRegistration);
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
