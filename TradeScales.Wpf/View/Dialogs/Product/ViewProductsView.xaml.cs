﻿using System;
using System.ComponentModel;
using TradeScales.Wpf.ViewModel.Dialogs;

namespace TradeScales.Wpf.View.Dialogs
{   
    public partial class ViewProductsView
    {

        #region Fields

        private ViewProductsViewModel _ViewModel;

        #endregion

        #region Constructor
     
        public ViewProductsView(ViewProductsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += ViewProductsView_Closing;
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

        private void ViewProductsView_Closing(object sender, CancelEventArgs e)
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
