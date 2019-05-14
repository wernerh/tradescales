using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WBS.Services.Utilities;

namespace WBS.Wpf.Resources.Services.Interfaces
{
    /// <summary>
    /// Dialog service interface
    /// </summary>
    public interface IDialogsService
    {

        string OpenFilePath { get; }
        string SaveFilePath { get; }

        bool? ShowOpenFileDialog(string filter);
        bool? ShowSaveFileDialog(string filter);
        bool? ShowSaveFileDialog(string filter, string filepath);
        bool? ShowFolderDialog(string folderpath);

        void ShowAboutDialog();
        void ShowOptionsDialog();
        MembershipContext ShowLogInDialog();
        void ShowActivateDialog();

        void ShowViewCustomersDialog();
        void ShowAddCustomerDialog(bool isEditCustomer, int id = -1, string code = null, string name = null);
        void ShowViewDestinationsDialog();
        void ShowAddDestinationDialog(bool isEditDestination, int id = -1, string code = null, string name = null);
        void ShowViewDriversDialog();
        void ShowAddDriverDialog(bool isEditDriver, int id = -1, string code = null, string firstname = null, string lastname = null);
        void ShowViewHauliersDialog();
        void ShowAddHaulierDialog(bool isEditHaulier, int id = -1, string code = null, string name = null);
        void ShowViewProductsDialog();
        void ShowAddProductDialog(bool isEditProduct, int id = -1, string code = null, string name = null);
        void ShowViewUsersDialog();
        void ShowAddUserDialog(bool isEditUser, int id = -1, string username = null, string email = null);
        void ShowViewVehiclesDialog();
        void ShowAddVehicleDialog(bool isEditVehicle, int id = -1, string code = null, string make = null, string registration = null);
    }
}
