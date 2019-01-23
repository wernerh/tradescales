using TradeScales.Model;
using TradeScales.Resources.Services.Interfaces;

namespace TradeScales.ViewModel.Tools
{
    /// <summary>
    /// Tool two viewmodel
    /// </summary>
    public class ToolTwoViewModel : ToolViewModel
    {
        
        #region Fields

        public const string ToolContentID = "ToolTwo";

        private static IMessageBoxService _MessageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ToolTwoViewModel class
        /// </summary>
        public ToolTwoViewModel()
            : base("Tool Two")
        {

            ContentID = ToolContentID;

        }

        #endregion

        #region Properties

        #endregion

        #region Commands

        #endregion

        #region Public Methods

        #endregion

    }
}
