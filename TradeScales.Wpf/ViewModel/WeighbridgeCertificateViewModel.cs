
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Wpf.ViewModel
{
    /// <summary>
    /// Weighbridge Certificate ViewModel viewmodel
    /// </summary>
    public class WeighbridgeCertificateViewModel : DocumentViewModel
    {

        #region Fields

        public const string ToolContentID = "WeighbridgeCertificate";

        #endregion

        #region Properties

        public string Filepath { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the WeighbridgeCertificateViewModel class.
        /// </summary>
        public WeighbridgeCertificateViewModel(string filepath, string ticketNumber)
            : base($"Weighbridge Certificate ({ticketNumber})")
        {
            ContentID = ToolContentID;
            Filepath = filepath;          
        }

        #endregion
    }
}
