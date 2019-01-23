using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services.Interfaces;

namespace TradeScales.Wpf.ViewModel
{
    /// <summary>
    /// tickets viewmodel
    /// </summary>
    public class TicketsViewModel : DocumentViewModel
    {

        #region Fields

        public const string ToolContentID = "Tickets";

        TradeScalesEntities context = new TradeScalesEntities();
        private static IMessageBoxService _MessageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        #endregion

        #region Properties


        public ObservableCollection<Ticket> Tickets { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the StartPageViewModel class.
        /// </summary>
        public TicketsViewModel()
            : base("Tickets")
        {
            ContentID = ToolContentID;
            context.Tickets.Load();
            Tickets = context.Tickets.Local;
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
                    _ViewCommand = new RelayCommand<Ticket>(GenerateTicket);
                }
                return _ViewCommand;
            }
        }

        #endregion

        #region Public Methods

        public void GenerateTicket(Ticket ticket)
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{ticket.TicketNumber}.pdf";

            // Create document
            Document document = new Document(PageSize.A4);
            var output = new FileStream(filePath, FileMode.Create);
            var writer = PdfWriter.GetInstance(document, output);
            document.Open();

            // Get logo path
            var logoPath = $"{rootPath}\\Resources\\images\\tradescales.png";

            // Read in the contents of the Receipt.htm file...
            string contents = File.ReadAllText($"{rootPath}\\Resources\\templates\\WeighbridgeTicketTemplate.html");

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
            document.Close();
        }

        #endregion

    }
}
