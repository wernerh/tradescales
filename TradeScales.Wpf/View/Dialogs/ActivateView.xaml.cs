using System;
using TradeScales.Wpf.ViewModel.Dialogs;

namespace TradeScales.Wpf.View.Dialogs
{
    /// <summary>
    /// Interaction logic for ActivateView.xaml
    /// </summary>
    public partial class ActivateView
    {

        #region Fields

        private ActivateViewModel _ViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the OptionsView 
        /// </summary>
        public ActivateView(ActivateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _ViewModel = viewModel;
            _ViewModel.RequestClose += RequestClose;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Manually closes a Window.
        /// </summary>
        /// <param name="sender">control/object that raised the event</param>
        /// <param name="e">parameter that contains the event data</param>
        public void RequestClose(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch { }
        }

        #endregion
    }
}
