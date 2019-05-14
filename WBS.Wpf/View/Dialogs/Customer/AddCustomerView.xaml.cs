using System;
using System.ComponentModel;
using WBS.Wpf.ViewModel.Dialogs;

namespace WBS.Wpf.View.Dialogs
{   
    public partial class AddCustomerView
    {

        #region Fields

        private AddCustomerViewModel _ViewModel;

        #endregion

        #region Constructor
     
        public AddCustomerView(AddCustomerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += AddCustomerView_Closing;
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

        private void AddCustomerView_Closing(object sender, CancelEventArgs e)
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
