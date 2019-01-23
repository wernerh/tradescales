using TradeScales.Model;
using TradeScales.Resources.Services.Interfaces;

namespace TradeScales.ViewModel.Tools
{
    /// <summary>
    /// Tool one viewmodel
    /// </summary>
    public class ToolOneViewModel : ToolViewModel
    {

        #region Fields

        public const string ToolContentID = "ToolOne";
        private static IMessageBoxService _MessageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        #endregion

        #region Properties

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ToolOneViewModel class
        /// </summary>
        public ToolOneViewModel()
            : base("Tool One")
        {

            
        }

        #endregion

        #region Commands

        #endregion

        #region Private Methods

        #endregion

    }
}
