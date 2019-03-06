using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeScales.Services.Utilities;

namespace TradeScales.Wpf.Resources.Services.Interfaces
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
        void ShowAddDestinationDialog();
        void ShowViewDriversDialog();
        void ShowAddDriverDialog();
        void ShowViewHauliersDialog();
        void ShowAddHaulierDialog();
        void ShowViewProductsDialog();
        void ShowAddProductDialog();
        void ShowViewUsersDialog();
        void ShowAddUserDialog();
        void ShowViewVehiclesDialog();
        void ShowAddVehicleDialog();
    }
}
