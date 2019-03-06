using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TradeScales.Services.Utilities;
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

        #region Constructor
    
        public MainViewModel()
        {
            ChangeAppTheme();
            LoadUserSettings();

            if (LoggedInUserContext == null)
            {
                LoggedInUserContext = _DialogService.ShowLogInDialog();
            }

            InitializeDocuments();
            UpdateDocumentFilePaths();
            StatusMessage = $"Welcome {LoggedInUserContext.User.Username}";
        }

        #endregion

        #region Properties

        private string _DialogTitle;
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

        public static string DirAppData
        {
            get
            {
                string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string directoryPath = $"{rootPath}\\{ HardCodedValues.CompanyName}\\{HardCodedValues.ApplicationName}";

                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                return directoryPath;
            }
        }

        private Theme _AvalonDockTheme;     
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
        public IEnumerable<ToolViewModel> Tools
        {
            get
            {
                if (_Tools == null)
                {
                    _Tools = new ToolViewModel[] { ToolOne, ToolTwo, ToolThree };
                }
                return _Tools;
            }
        }

        private ToolOneViewModel _ToolOne;    
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

        private ToolThreeViewModel _ToolThree;  
        public ToolThreeViewModel ToolThree
        {
            get
            {
                if (_ToolThree == null)
                {
                    _ToolThree = new ToolThreeViewModel();
                }
                return _ToolThree;
            }
            set
            {
                _ToolThree = value;
            }
        }

        public MembershipContext LoggedInUserContext { get; set; }

        #endregion

        #endregion

        #region Commands

        #region App
        private ICommand _ViewStartPageCommand;      
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

        private ICommand _LogOutCommand;
        public ICommand LogOutCommand
        {
            get
            {
                if (_LogOutCommand == null)
                {
                    _LogOutCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                LoggedInUserContext = null;
                                LoggedInUserContext = _DialogService.ShowLogInDialog();
                                StatusMessage = $"Welcome back {LoggedInUserContext.User.Username}";
                            }
                            catch (Exception ex)
                            {
                                ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _LogOutCommand;
            }
        }

        private ICommand _ExitCommand;      
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

        private ICommand _ViewCustomersCommand;
        public ICommand ViewCustomersCommand
        {
            get
            {
                if (_ViewCustomersCommand == null)
                {
                    _ViewCustomersCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowViewCustomersDialog();
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _ViewCustomersCommand;
            }
        }

        private ICommand _AddCustomerCommand;
        public ICommand AddCustomerCommand
        {
            get
            {
                if (_AddCustomerCommand == null)
                {
                    _AddCustomerCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowAddCustomerDialog(false);
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _AddCustomerCommand;
            }
        }

        private ICommand _ViewDestinationsCommand;
        public ICommand ViewDestinationsCommand
        {
            get
            {
                if (_ViewDestinationsCommand == null)
                {
                    _ViewDestinationsCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowViewDestinationsDialog();
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _ViewDestinationsCommand;
            }
        }

        private ICommand _AddDestinationCommand;
        public ICommand AddDestinationCommand
        {
            get
            {
                if (_AddDestinationCommand == null)
                {
                    _AddDestinationCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowAddDestinationDialog(false);
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _AddDestinationCommand;
            }
        }

        private ICommand _ViewDriversCommand;
        public ICommand ViewDriversCommand
        {
            get
            {
                if (_ViewDriversCommand == null)
                {
                    _ViewDriversCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowViewDriversDialog();
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _ViewDriversCommand;
            }
        }

        private ICommand _AddDriverCommand;
        public ICommand AddDriverCommand
        {
            get
            {
                if (_AddDriverCommand == null)
                {
                    _AddDriverCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowAddDriverDialog(false);
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _AddDriverCommand;
            }
        }

        private ICommand _ViewHauliersCommand;
        public ICommand ViewHauliersCommand
        {
            get
            {
                if (_ViewHauliersCommand == null)
                {
                    _ViewHauliersCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowViewHauliersDialog();
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _ViewHauliersCommand;
            }
        }

        private ICommand _AddHaulierCommand;
        public ICommand AddHaulierCommand
        {
            get
            {
                if (_AddHaulierCommand == null)
                {
                    _AddHaulierCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowAddHaulierDialog(false);
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _AddHaulierCommand;
            }
        }

        private ICommand _ViewProductsCommand;
        public ICommand ViewProductsCommand
        {
            get
            {
                if (_ViewProductsCommand == null)
                {
                    _ViewProductsCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowViewProductsDialog();
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _ViewProductsCommand;
            }
        }

        private ICommand _AddProductCommand;
        public ICommand AddProductCommand
        {
            get
            {
                if (_AddProductCommand == null)
                {
                    _AddProductCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowAddProductDialog(false);                          
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _AddProductCommand;
            }
        }

        private ICommand _ViewUsersCommand;
        public ICommand ViewUsersCommand
        {
            get
            {
                if (_ViewUsersCommand == null)
                {
                    _ViewUsersCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowViewUsersDialog();
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _ViewUsersCommand;
            }
        }

        private ICommand _AddUserCommand;
        public ICommand AddUserCommand
        {
            get
            {
                if (_AddUserCommand == null)
                {
                    _AddUserCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowAddUserDialog(false);
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _AddUserCommand;
            }
        }

        private ICommand _ViewVehiclesCommand;
        public ICommand ViewVehiclesCommand
        {
            get
            {
                if (_ViewVehiclesCommand == null)
                {
                    _ViewVehiclesCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowViewVehiclesDialog();
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _ViewVehiclesCommand;
            }
        }

        private ICommand _AddVehicleCommand;
        public ICommand AddVehicleCommand
        {
            get
            {
                if (_AddVehicleCommand == null)
                {
                    _AddVehicleCommand = new MVVMRelayCommand(execute =>
                    {
                        try
                        {
                            _DialogService.ShowAddVehicleDialog(false);
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessageBox(ex);
                        }
                    });
                }
                return _AddVehicleCommand;
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

        public void ReloadEntities()
        {
            ToolTwo.ReloadEntities();
            ToolThree.ReloadEntities();

            foreach(var document in Documents)
            {
                if(document.ContentID.Contains("New") || document.ContentID.Contains("Edit"))
                {
                    document.ReloadEntities();
                }
            }          
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
            ToolThree.IsVisible = Settings.Default.ToolThreeIsVisible;
        }

        private void SaveUserSettings()
        {
            Settings.Default.ToolOneIsVisible = ToolOne.IsVisible;
            Settings.Default.ToolTwoIsVisible = ToolTwo.IsVisible;
            Settings.Default.ToolThreeIsVisible = ToolThree.IsVisible;
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

            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string directoryPath = $"{rootPath}\\{ HardCodedValues.CompanyName}\\{HardCodedValues.ApplicationName}";

            if (string.IsNullOrEmpty(weighbridgeCertificateFolder) && string.IsNullOrEmpty(reportsFolder))
            {
                Settings.Default.WeighBridgeCertificatesFolder = $"{directoryPath}\\WeighBridgeCertificates";
                Settings.Default.WeighBridgeCertificateLogo = $"{directoryPath}\\images\\tradescales.png";
                Settings.Default.WeighBridgeCertificateTemplate = $"{directoryPath}\\templates\\WeighbridgeTicketTemplate.html";
                Settings.Default.ReportsFolder = $"{directoryPath}\\Reports";
                Settings.Default.ReportLogo = $"{directoryPath}\\images\\tradescales.png";
                Settings.Default.ReportTemplate = $"{directoryPath}\\templates\\ReportTemplate.html";
                Settings.Default.Save();
            }
        }

        #endregion

    }
}
