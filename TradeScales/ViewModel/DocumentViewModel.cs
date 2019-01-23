namespace TradeScales.ViewModel
{
    /// <summary>
    /// </summary>
    public class DocumentViewModel : PaneViewModel
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DocumentViewModel class.
        /// </summary>
        /// <param name="name"></param>
        public DocumentViewModel(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        private string _Name;
        /// <summary>
        /// </summary>
        public string Name
        {
            get { return (IsDirty) ? _Name + "*" : _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private bool _IsDirty;
        /// <summary>
        /// </summary>
        public bool IsDirty
        {
            get { return _IsDirty; }
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    OnPropertyChanged("IsDirty");
                    OnPropertyChanged("Name");
                }
            }
        }

        #endregion

    }
}
