using System;
using System.ComponentModel;
using TradeScales.Wpf.ViewModel.Dialogs;

namespace TradeScales.Wpf.View.Dialogs
{   
    public partial class ViewHauliersView
    {

        #region Fields

        private ViewHauliersViewModel _ViewModel;

        #endregion

        #region Constructor
     
        public ViewHauliersView(ViewHauliersViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += ViewHauliersView_Closing;
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

        private void ViewHauliersView_Closing(object sender, CancelEventArgs e)
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
