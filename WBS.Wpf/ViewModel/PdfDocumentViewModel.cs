namespace WBS.Wpf.ViewModel
{
    /// <summary>
    /// Pdf document viewmodel
    /// </summary>
    public class PdfDocumentViewModel : DocumentViewModel
    {

        #region Fields

        public const string ToolContentID = "PdfDocument";

        #endregion

        #region Properties

        public string FilePath { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Pdf document viewmodel class
        /// </summary>
        public PdfDocumentViewModel(string filePath)
            : base($"{filePath}")
        {
            ContentID = ToolContentID;
            FilePath = filePath;          
        }

        #endregion
    }
}
