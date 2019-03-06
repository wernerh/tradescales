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

        public void ShowAddCustomerDialog()
        {
            AddCustomerViewModel viewmodel = new AddCustomerViewModel();
            AddCustomerView view = new AddCustomerView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddDestinationDialog()
        {
            AddDestinationViewModel viewmodel = new AddDestinationViewModel();
            AddDestinationView view = new AddDestinationView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddDriverDialog()
        {
            AddDriverViewModel viewmodel = new AddDriverViewModel();
            AddDriverView view = new AddDriverView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddHaulierDialog()
        {
            AddHaulierViewModel viewmodel = new AddHaulierViewModel();
            AddHaulierView view = new AddHaulierView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddProductDialog()
        {
            AddProductViewModel viewmodel = new AddProductViewModel();
            AddProductView view = new AddProductView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddUserDialog()
        {
            AddUserViewModel viewmodel = new AddUserViewModel();
            AddUserView view = new AddUserView(viewmodel);
            view.ShowDialog();
        }

        public void ShowAddVehicleDialog()
        {
            AddVehicleViewModel viewmodel = new AddVehicleViewModel();
            AddVehicleView view = new AddVehicleView(viewmodel);
            view.ShowDialog();
        }

        #endregion

    }
}
