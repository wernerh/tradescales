using Autofac;
using System.Windows;
using System.Windows.Threading;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Resources.Services;
using TradeScales.Wpf.Resources.Services.Interfaces;
using TradeScales.Wpf.View;
using TradeScales.WPF.Mappings;

namespace TradeScales.Wpf
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
