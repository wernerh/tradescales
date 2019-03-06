using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.IO;
using TradeScales.Services.Utilities;
using TradeScales.Wpf.Resources.Services.Interfaces;
using TradeScales.Wpf.View.Dialogs;
using TradeScales.Wpf.ViewModel.Dialogs;

namespace TradeScales.Wpf.Resources.Services
{
    /// <summary>
    /// Dialog service.
    /// </summary>
    public class DialogService : IDialogsService
    {
        #region Fields

        private OpenFileDialog _OpenFileDialog = new OpenFileDialog();
        private SaveFileDialog _SaveFileDialog = new SaveFileDialog();

        #endregion

        #region Properties

        public string OpenFilePath { get; set; }
  
        public string SaveFilePath { get; set; }

        #endregion

        #region Public Methods

        public bool? ShowOpenFileDialog(string filter)
        {
            _OpenFileDialog.Filter = filter;
            bool? result = _OpenFileDialog.ShowDialog();
            OpenFilePath = _OpenFileDialog.FileName;
            return result;
        }

        public bool? ShowSaveFileDialog(string filter)
        {
            _SaveFileDialog.Filter = filter;
            bool? result = _SaveFileDialog.ShowDialog();
            SaveFilePath = _SaveFileDialog.FileName;
            return result;
        }

        public bool? ShowSaveFileDialog(string filter, string filepath)
        {
            string filename = Path.GetFileNameWithoutExtension(filepath);
            string folderPath = Directory.GetParent(filepath).FullName;

            _SaveFileDialog.Filter = filter;
            _SaveFileDialog.FileName = filename;
            _SaveFileDialog.InitialDirectory = folderPath;

            _SaveFileDialog.RestoreDirectory = true;

            bool? result = _SaveFileDialog.ShowDialog();

            SaveFilePath = _SaveFileDialog.FileName;

            return result;
        }

        public bool? ShowFolderDialog(string folderpath)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.DefaultDirectory = folderpath;
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                SaveFilePath = dialog.FileName;
                return true;
            }

            return null;
        }
        
        public void ShowAboutDialog()
        {
            AboutView view = new AboutView();
            view.ShowDialog();
        }
             
        public void ShowOptionsDialog()
        {
            OptionsViewModel viewmodel = new OptionsViewModel();
            OptionsView view = new OptionsView(viewmodel);
            view.ShowDialog();
        }

        public MembershipContext ShowLogInDialog()
        {
            LogInViewModel viewmodel = new LogInViewModel();
            LogInView view = new LogInView(viewmodel);

            view.ShowDialog();

            return viewmodel.LoggedInUserContext;
        }

        public void ShowActivateDialog()
        {
            ActivateViewModel viewmodel = new ActivateViewModel();
            ActivateView view = new ActivateView(viewmodel);

            view.ShowDialog();       
        }

        public void ShowViewCustomersDialog()
        {
            ViewCustomersViewModel viewmodel = new ViewCustomersViewModel();
            ViewCustomersView view = new ViewCustomersView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddCustomerDialog(bool isEditCustomer, int id = -1, string code = null, string name = null)
        {
            AddCustomerViewModel viewmodel = new AddCustomerViewModel(isEditCustomer, id, code, name);
            AddCustomerView view = new AddCustomerView(viewmodel);
            view.ShowDialog();
        }

        public void ShowViewDestinationsDialog()
        {
            ViewDestinationsViewModel viewmodel = new ViewDestinationsViewModel();
            ViewDestinationsView view = new ViewDestinationsView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddDestinationDialog(bool isEditDestination, int id = -1, string code = null, string name = null)
        {
            AddDestinationViewModel viewmodel = new AddDestinationViewModel(isEditDestination, id, code, name);
            AddDestinationView view = new AddDestinationView(viewmodel);
            view.ShowDialog();
        }


        public void ShowViewDriversDialog()
        {
            ViewDriversViewModel viewmodel = new ViewDriversViewModel();
            ViewDriversView view = new ViewDriversView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddDriverDialog(bool isEditDriver, int id = -1, string code = null, string firstname = null, string lastname = null)
        {
            AddDriverViewModel viewmodel = new AddDriverViewModel(isEditDriver, id, code, firstname, lastname);
            AddDriverView view = new AddDriverView(viewmodel);
            view.ShowDialog();
        }

        public void ShowViewHauliersDialog()
        {
            ViewHauliersViewModel viewmodel = new ViewHauliersViewModel();
            ViewHauliersView view = new ViewHauliersView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddHaulierDialog(bool isEditHaulier, int id = -1, string code = null, string name = null)
        {
            AddHaulierViewModel viewmodel = new AddHaulierViewModel(isEditHaulier, id, code, name);
            AddHaulierView view = new AddHaulierView(viewmodel);
            view.ShowDialog();
        }

        public void ShowViewProductsDialog()
        {
            ViewProductsViewModel viewmodel = new ViewProductsViewModel();
            ViewProductsView view = new ViewProductsView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddProductDialog(bool isEditProduct, int id = -1, string code = null, string name = null)
        {
            AddProductViewModel viewmodel = new AddProductViewModel(isEditProduct, id, code, name);
            AddProductView view = new AddProductView(viewmodel);
            view.ShowDialog();
        }

        public void ShowViewUsersDialog()
        {
            ViewUsersViewModel viewmodel = new ViewUsersViewModel();
            ViewUsersView view = new ViewUsersView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddUserDialog(bool isEditUser, int id = -1, string username = null, string email = null)
        {
            AddUserViewModel viewmodel = new AddUserViewModel(isEditUser, id, username, email);
            AddUserView view = new AddUserView(viewmodel);
            view.ShowDialog();
        }

        public void ShowViewVehiclesDialog()
        {
            ViewVehiclesViewModel viewmodel = new ViewVehiclesViewModel();
            ViewVehiclesView view = new ViewVehiclesView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddVehicleDialog(bool isEditVehicle, int id = -1, string code = null, string make = null, string registration = null)
        {
            AddVehicleViewModel viewmodel = new AddVehicleViewModel(isEditVehicle, id, code, make, registration);
            AddVehicleView view = new AddVehicleView(viewmodel);
            view.ShowDialog();
        }

        #endregion

    }
}
