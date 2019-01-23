using System.Collections.ObjectModel;
using System.Data.Entity;
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

        #region Public Methods

        #endregion

    }
}
