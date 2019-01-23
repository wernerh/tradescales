using System;
using System.ComponentModel;
using TradeScales.Wpf.ViewModel.Dialogs;

namespace TradeScales.Wpf.View.Dialogs
{
    /// <summary>
    /// Interaction logic for OptionsView.xaml
    /// </summary>
    public partial class OptionsView
    {

        #region Fields

        private OptionsViewModel _ViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the OptionsView 
        /// </summary>
        public OptionsView(OptionsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += OptionsView_Closing;
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

        #region Private Methods

        private void OptionsView_Closing(object sender, CancelEventArgs e)
        {
            if (_ViewModel.NotXClosed)
            {
                return;
            }

            _ViewModel.CancelCommand.Execute(null);
        }

        #endregion

    }
}
