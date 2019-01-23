using System.Text;
using TradeScales.Model;
using TradeScales.Resources.Services.Interfaces;

namespace TradeScales.ViewModel
{
    /// <summary>
    /// Start page viewmodel
    /// </summary>
    public class StartPageViewModel : DocumentViewModel
    {

        #region Fields

        /// <summary>
        /// Used to identify the start page in avalon dock
        /// </summary>
        public const string ToolContentID = "StartPage";

        /// <summary>
        /// Message box service
        /// </summary>
        private static IMessageBoxService _MessageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the StartPageViewModel class.
        /// </summary>
        public StartPageViewModel()
            : base("Start Page")
        {
            ContentID = ToolContentID;
        }

        #endregion

        #region Properties

        private string _MessageLog;
        // Display ReadMe.txt to user
        public string MessageLog
        {
            get { return _MessageLog; }
            set
            {
                _MessageLog = value;
                OnPropertyChanged("MessageLog");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add message to display
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            StringBuilder sb = new StringBuilder(MessageLog);
            sb.AppendLine(message);
            MessageLog = sb.ToString();
        }

        public void ClearMessageLog()
        {           
            MessageLog = "";
        }

        #endregion

    }
}
