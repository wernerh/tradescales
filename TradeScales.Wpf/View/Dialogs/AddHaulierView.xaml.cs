﻿using System;
using System.ComponentModel;
using TradeScales.Wpf.ViewModel.Dialogs;

namespace TradeScales.Wpf.View.Dialogs
{   
    public partial class AddHaulierView
    {

        #region Fields

        private AddHaulierViewModel _ViewModel;

        #endregion

        #region Constructor
     
        public AddHaulierView(AddHaulierViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += AddHaulierView_Closing;
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

        private void AddHaulierView_Closing(object sender, CancelEventArgs e)
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
