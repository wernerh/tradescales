using System;
using System.ComponentModel;
using WBS.Wpf.ViewModel.Dialogs;

namespace WBS.Wpf.View.Dialogs
{
    /// <summary>
    /// Interaction logic for LogInView.xaml
    /// </summary>
    public partial class LogInView
    {

        #region Fields

        private LogInViewModel _ViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the OptionsView 
        /// </summary>
        public LogInView(LogInViewModel viewModel)
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
