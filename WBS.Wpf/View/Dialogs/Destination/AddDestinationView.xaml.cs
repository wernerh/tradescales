using System;
using System.ComponentModel;
using WBS.Wpf.ViewModel.Dialogs;

namespace WBS.Wpf.View.Dialogs
{   
    public partial class AddDestinationView
    {

        #region Fields

        private AddDestinationViewModel _ViewModel;

        #endregion

        #region Constructor
     
        public AddDestinationView(AddDestinationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += AddDestinationView_Closing;
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

        private void AddDestinationView_Closing(object sender, CancelEventArgs e)
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
