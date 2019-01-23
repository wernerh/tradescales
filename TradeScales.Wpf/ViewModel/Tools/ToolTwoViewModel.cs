using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services.Interfaces;

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

        #endregion

        #region Properties

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

        #region Commands

        #endregion

        #region Public Methods

        #endregion

    }
}
