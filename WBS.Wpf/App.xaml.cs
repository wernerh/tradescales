﻿using Autofac;
using System.Windows;
using System.Windows.Threading;
using WBS.Wpf.Model;
using WBS.Wpf.Resources.Services;
using WBS.Wpf.Resources.Services.Interfaces;
using WBS.WPF.Mappings;

namespace WBS.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region Fields

        private static DialogService _DialogService = new DialogService();
        private static MessageBoxService _MessageBoxService = new MessageBoxService();
  
        #endregion

        #region Properties

        private static IContainer Container { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            ServiceLocator.Instance.AddService<IDialogsService>(_DialogService);
            ServiceLocator.Instance.AddService<IMessageBoxService>(_MessageBoxService);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
           
            BootStrapper.Start();
            AutoMapperConfiguration.Configure();

            base.OnStartup(e);
        }

        #endregion

        #region Private Methods

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _MessageBoxService.ShowExceptionMessageBox(e.Exception, "Fatal Error", MessageBoxImage.Error);
        }

        #endregion

    }
}