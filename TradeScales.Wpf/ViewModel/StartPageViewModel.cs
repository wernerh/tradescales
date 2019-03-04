using System.IO;
using System.Reflection;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Properties;
using TradeScales.Wpf.Resources.Services.Interfaces;

namespace TradeScales.Wpf.ViewModel
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

        #region Properties

        public string ImagePath
        {
            get { return Settings.Default.WeighBridgeCertificateLogo; }       
        }

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

    }
}
