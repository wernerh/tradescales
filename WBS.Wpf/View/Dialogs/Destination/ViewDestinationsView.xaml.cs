using System;
using System.ComponentModel;
using WBS.Wpf.ViewModel.Dialogs;

namespace WBS.Wpf.View.Dialogs
{   
    public partial class ViewDestinationsView
    {

        #region Fields

        private ViewDestinationsViewModel _ViewModel;

        #endregion

        #region Constructor
     
        public ViewDestinationsView(ViewDestinationsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += ViewDestinationsView_Closing;
            _ViewModel = viewModel;
            _ViewModel.RequestClose += RequestClose;

        }

        #endregion

        #region Public Methods
    
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

        private void ViewDestinationsView_Closing(object sender, CancelEventArgs e)
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
