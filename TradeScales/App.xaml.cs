using System.Windows;
using System.Windows.Threading;
using TradeScales.Model;
using TradeScales.Resources.Services;
using TradeScales.Resources.Services.Interfaces;

namespace TradeScales
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
            IoCContainer.Build();
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
