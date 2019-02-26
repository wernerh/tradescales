using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Properties;
using TradeScales.Wpf.Resources.Services.Interfaces;
using TradeScales.Wpf.ViewModel.Tools;
using Xceed.Wpf.AvalonDock.Themes;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields

        private static IDialogsService _DialogService = ServiceLocator.Instance.GetService<IDialogsService>();
        private static IMessageBoxService _MessageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        #endregion

        #region Properties

        private string _DialogTitle;
        /// <summary>
        /// Application Title
        /// </summary>
        public string DialogTitle
        {
            get
            {
                if (_DialogTitle == null)
                {
                    _DialogTitle = $"{HardCodedValues.ApplicationName} {Assembly.GetEntryAssembly().GetName().Version}";
                }
                return _DialogTitle;
            }
        }

        private string _StatusMessage;
        /// <summary>
        /// Application status message 
        /// </summary>
        public string StatusMessage
        {
            get { return _StatusMessage; }
            set
            {
                if (_StatusMessage != value)
                {
                    _StatusMessage = value;
                    OnPropertyChanged("StatusMessage");
                }
            }
        }

        private static MainViewModel _This;
        /// <summary>
        /// Static instance of this main view model class (Singleton pattern)
        /// </summary>
        public static MainViewModel This
        {
            get { return _This; }
            set
            {
                if (_This != value)
                {
                    _This = value;
                }
            }
        }

        private AvalonDockLayoutViewModel _Layout;
        /// <summary>
        /// Used to load or save AvalonDock layout on application startup and shut-down
        /// </summary>
        public AvalonDockLayoutViewModel Layout
        {
            get
            {
                if (_Layout == null)
                {
                    _Layout = new AvalonDockLayoutViewModel();
                }

                return _Layout;
            }
        }

        /// <summary>
        /// Directory where "Layout.config" is saved ("C:\Users\{User}\AppData\Roaming\Werner Hurter\Trade Scales")
        /// </summary>
        public static string DirAppData
        {
            get
            {
                string directoryPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{HardCodedValues.CompanyName}\\{HardCodedValues.ApplicationName}";

                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                return directoryPath;
            }
        }

        private Theme _AvalonDockTheme;
        /// <summary>
        /// Avalon dock theme enumeration
        /// </summary>
        public Theme AvalonDockTheme
        {
            get { return _AvalonDockTheme; }
            set
            {
                if (_AvalonDockTheme != value)
                {
                    _AvalonDockTheme = value;
                    OnPropertyChanged("AvalonDockTheme");
                }
            }
        }

        private ObservableCollection<DocumentViewModel> _Documents;
        /// <summary>
        /// Collection of files open in the application
        /// </summary>
        public ObservableCollection<DocumentViewModel> Documents
        {
            get { return _Documents; }
            set
            {
                if (_Documents != value)
                {
                    _Documents = value;
                    OnPropertyChanged("Documents");
                }
            }
        }

        private DocumentViewModel _ActiveDocument = null;
        /// <summary>
        /// The active document
        /// </summary>
        public DocumentViewModel ActiveDocument
        {
            get { return _ActiveDocument; }
            set
            {
                if (_ActiveDocument != value)
                {
                    _ActiveDocument = value;
                    OnPropertyChanged("ActiveDocument");

                    DocumentChanged();
                    ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private StartPageViewModel _StartPage;
        /// <summary>
        /// The start page
        /// </summary>
        public StartPageViewModel StartPage
        {
            get
            {
                if (_StartPage == null)
                {
                    _StartPage = new StartPageViewModel();
                }
                return _StartPage;
            }
        }

        private TicketListViewModel _TicketList;
        /// <summary>
        /// The list of tickets
        /// </summary>
        public TicketListViewModel TicketList
        {
            get
            {
                if (_TicketList == null)
                {
                    _TicketList = new TicketListViewModel();
                }
                return _TicketList;
            }
        }

        private NewTicketViewModel _NewTicket;
        /// <summary>
        /// New ticket
        /// </summary>
        public NewTicketViewModel NewTicket
        {
            get
            {
                if (_NewTicket == null)
                {
                    _NewTicket = new NewTicketViewModel();
                }
                return _NewTicket;
            }
        }

        #region Tools

        private ToolViewModel[] _Tools;
        /// <summary>
        /// Collection of all the available tool windows
        /// </summary>
        public IEnumerable<ToolViewModel> Tools
        {
            get
            {
                if (_Tools == null)
                {
                    _Tools = new ToolViewModel[] { ToolOne, ToolTwo };
                }
                return _Tools;
            }
        }

        private ToolOneViewModel _ToolOne;
        /// <summary>   
        /// Tool one 
        /// </summary>
        public ToolOneViewModel ToolOne
        {
            get
            {
                if (_ToolOne == null)
                {
                    _ToolOne = new ToolOneViewModel();
                    _ToolOne.IsActive = true;
                    _ToolOne.IsVisible = true;
                }
                return _ToolOne;
            }
        }

        private ToolTwoViewModel _ToolTwo;
        /// <summary>   
        /// Tool two
        /// </summary>
        public ToolTwoViewModel ToolTwo
        {
            get
            {
                if (_ToolTwo == null)
                {
                    _ToolTwo = new ToolTwoViewModel();
                }
                return _ToolTwo;
            }
            set
            {
                _ToolTwo = value;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ChangeAppTheme();
            LoadUserSettings();
            InitializeDocuments();
            UpdateDocumentFilePaths();
            StatusMessage = "Welcome to Trade Scales";
        }

        #endregion

        #region Commands

        private ICommand _ViewStartPageCommand;
        /// <summary>
        /// View start page command
        /// </summary>
        public ICommand ViewStartPageCommand
        {
            get
            {
                if (_ViewStartPageCommand == null)
                {
                    _ViewStartPageCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                DocumentViewModel document = Documents.FirstOrDefault(e => e.ContentID == StartPageViewModel.ToolContentID);

                                if (document != null)
                                {
                                    ActiveDocument = document;
                                    return;
                                }

                                Documents.Add(StartPage);
                                ActiveDocument = StartPage;
                            }
                            catch (Exception ex)
                            {
                                ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _ViewStartPageCommand;
            }
        }

        private ICommand _TicketListCommand;
        /// <summary>
        /// Ticket list command
        /// </summary>
        public ICommand TicketListCommand
        {
            get
            {
                if (_TicketListCommand == null)
                {
                    _TicketListCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                OpenTicketList();
                            }
                            catch (Exception ex)
                            {
                                ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _TicketListCommand;
            }
        }

        private ICommand _NewTicketCommand;
        /// <summary>
        /// New ticket command
        /// </summary>
        public ICommand NewTicketCommand
        {
            get
            {
                if (_NewTicketCommand == null)
                {
                    _NewTicketCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                OpenNewTicket();
                            }
                            catch (Exception ex)
                            {
                                ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _NewTicketCommand;
            }
        }

        private ICommand _AboutCommand;
        /// <summary>
        /// Launch about dialog
        /// </summary>
        public ICommand AboutCommand
        {
            get
            {
                if (_AboutCommand == null)
                {
                    _AboutCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                _DialogService.ShowAboutDialog();
                            }
                            catch (Exception ex)
                            {
                                ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _AboutCommand;
            }
        }

        private ICommand _ThemeOptionsCommand;
        /// <summary>
        /// Launch theme options dialog
        /// </summary>
        public ICommand ThemeOptionsCommand
        {
            get
            {
                if (_ThemeOptionsCommand == null)
                {
                    _ThemeOptionsCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowOptionsDialog();
                            ChangeAppTheme();
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _ThemeOptionsCommand;
            }
        }

        private ICommand _ExitCommand;
        /// <summary>
        /// Shut down the application command
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                if (_ExitCommand == null)
                {
                    _ExitCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                OnRequestClose();
                            }
                            catch (Exception ex)
                            {
                                ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _ExitCommand;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the application should be closed
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Raised when the active display has changed
        /// </summary>
        public event EventHandler ActiveDocumentChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// Change the theme of the app
        /// Application theme options: BaseDark, BaseLight and Metro
        /// Application accent options: Red, Green, Blue, Purple, Orange, Lime, Emerald, Teal, Cyan, Cobalt, Indigo, Violet, Pink, Magenta, Metro, Crimson, Amber, Yellow, Brown, Olive, Steel, Mauve, Taupe and Sienna
        /// Workbench options: Dark, Light, Generic, Metro, VS2010 and Aero
        /// </summary>
        public void ChangeAppTheme()
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(Settings.Default.ThemeAccent), ThemeManager.GetAppTheme(Settings.Default.Theme));
            ChangeWorkbenchTheme();
        }

        /// <summary>
        /// Ask the user if he really intended to close the application
        /// </summary>
        public MessageBoxResult Exit()
        {
            SaveUserSettings();
            return MessageBoxResult.None;
        }

        public void OpenTicketList()
        {
            var tickeListViewModel = Documents.FirstOrDefault(e => e.ContentID == TicketListViewModel.ToolContentID);
            if (tickeListViewModel != null)
            {
                Documents.Remove(tickeListViewModel);
            }

            Documents.Add(TicketList);
            ActiveDocument = TicketList;
        }

        public void OpenNewTicket()
        {
            var newTicketViewModel = Documents.FirstOrDefault(e => e.ContentID == NewTicketViewModel.ToolContentID);
            if (newTicketViewModel != null)
            {
                Documents.Remove(newTicketViewModel);
            }

            Documents.Add(NewTicket);
            ActiveDocument = NewTicket;
        }

        public void OpenEditTicket(TicketViewModel ticket)
        {
            EditTicketViewModel editTicketViewModel = new EditTicketViewModel(ticket);

            var openEditTicketViewModel = Documents.FirstOrDefault(x => x.Name == editTicketViewModel.Name);
            if (openEditTicketViewModel != null)
            {
                Documents.Remove(openEditTicketViewModel);
            }

            Documents.Add(editTicketViewModel);
            ActiveDocument = editTicketViewModel;
        }

        public void OpenPdfDocument(string filePath)
        {
            PdfDocumentViewModel document = new PdfDocumentViewModel(filePath);

            var existingDocument = Documents.FirstOrDefault(x => x.Name == document.Name);
            if (existingDocument != null)
            {
                Documents.Remove(existingDocument);
            }

            Documents.Add(document);
            ActiveDocument = document;
        }

        public void ShowExceptionMessageBox(Exception ex)
        {
            _MessageBoxService.ShowExceptionMessageBox(ex, "Error", MessageBoxImage.Error);
        }

        #endregion

        #region Private Methods

        private void InitializeDocuments()
        {
            _Documents = new ObservableCollection<DocumentViewModel>();

            Documents.Add(TicketList);
            Documents.Add(NewTicket);
            ActiveDocument = TicketList;

            if (Settings.Default.ShowStartPageOnStartup)
            {
                if (!Documents.Contains(StartPage))
                {
                    Documents.Add(StartPage);
                }

                ActiveDocument = StartPage;
                return;
            }

            if (Documents.Contains(StartPage))
            {
                Documents.Remove(StartPage);
            }
        }

        private void LoadUserSettings()
        {
            ToolOne.IsVisible = Settings.Default.ToolOneIsVisible;
            ToolTwo.IsVisible = Settings.Default.ToolTwoIsVisible;
        }

        private void SaveUserSettings()
        {
            Settings.Default.ToolOneIsVisible = ToolOne.IsVisible;
            Settings.Default.ToolTwoIsVisible = ToolTwo.IsVisible;
            Settings.Default.Save();
        }

        private void ChangeWorkbenchTheme()
        {
            switch (Settings.Default.WorkBenchTheme)
            {
                case "Dark":
                    AvalonDockTheme = new ExpressionDarkTheme();
                    break;

                case "Light":
                    AvalonDockTheme = new ExpressionLightTheme();
                    break;

                case "Generic":
                    AvalonDockTheme = new GenericTheme();
                    break;

                case "Metro":
                    AvalonDockTheme = new MetroTheme();
                    break;

                case "VS2010":
                    AvalonDockTheme = new VS2010Theme();
                    break;

                case "Aero":
                    AvalonDockTheme = new AeroTheme();
                    break;
            }
        }

        private void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void DocumentChanged()
        {
            ToolOne.ActiveDocument = ActiveDocument.ToString();
        }

        private void UpdateDocumentFilePaths()
        {
            var weighbridgeCertificateFolder = Settings.Default.WeighBridgeCertificatesFolder;
            var reportsFolder = Settings.Default.ReportsFolder;

            if (string.IsNullOrEmpty(weighbridgeCertificateFolder) && string.IsNullOrEmpty(reportsFolder))
            {
                string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                Settings.Default.WeighBridgeCertificatesFolder = $"{rootPath}\\WeighBridgeCertificates";
                Settings.Default.WeighBridgeCertificateLogo = $"{rootPath}\\Resources\\images\\tradescales.png";
                Settings.Default.WeighBridgeCertificateTemplate = $"{rootPath}\\Resources\\templates\\WeighbridgeTicketTemplate.html";
                Settings.Default.ReportsFolder = $"{rootPath}\\Reports";
                Settings.Default.ReportLogo = $"{rootPath}\\Resources\\images\\tradescales.png";
                Settings.Default.ReportTemplate = $"{rootPath}\\Resources\\templates\\ReportTemplate.html";
                Settings.Default.Save();
            }
        }

        #endregion

    }
}
