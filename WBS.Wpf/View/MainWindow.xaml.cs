using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Entities;
using WBS.Wpf.ViewModel;
using Xceed.Wpf.AvalonDock;

namespace WBS.Wpf.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel.This = new MainViewModel();      
            MainViewModel.This.RequestClose += RequestClose;    
            DataContext = MainViewModel.This;
            Loaded += MainWindow_Loaded;
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
                BootStrapper.Stop();
                Close();
            }

            catch { }
        }

        #endregion

        #region Private Methods

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MainViewModel.This.Exit();

            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void DockingManager_DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            DocumentViewModel model = e.Document.Content as DocumentViewModel;

            if (model == null) return;

            if (model.ContentID == StartPageViewModel.ToolContentID)
            {
                Dispatcher.BeginInvoke(new Action(() => MainViewModel.This.Documents.Remove(model)), DispatcherPriority.Background);
                return;
            }       
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel.This.IsUserLoggedIn();
        }


        #endregion Private Methods

    }
}
