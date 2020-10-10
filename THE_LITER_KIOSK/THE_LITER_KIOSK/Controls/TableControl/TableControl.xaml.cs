using System.Windows;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.TableControl
{
    /// <summary>
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : CustomControlModel
    {
        public TableControl()
        {
            InitializeComponent();
            Loaded += TableControl_Loaded;
        }

        private async void TableControl_Loaded(object sender, RoutedEventArgs e)
        {
            await App.placeData.tableViewModel.LoadTableData();
            this.DataContext = App.placeData.tableViewModel;
        }

        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PLACE);
        }

        private void btnMoveToPay_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }
    }
}
