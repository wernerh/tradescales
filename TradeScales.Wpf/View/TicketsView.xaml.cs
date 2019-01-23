using System.Windows.Controls;

namespace TradeScales.Wpf.View
{
    /// <summary>
    /// Interaction logic for TicketsView.xaml
    /// </summary>
    public partial class TicketsView : UserControl
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the StartUpView class
        /// </summary>
        public TicketsView()
        {
            InitializeComponent();
        }

        #endregion

        private void UserControl_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
        {

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }
    }
}
