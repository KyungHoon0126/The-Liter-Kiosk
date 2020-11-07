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

        private void TableControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.placeData.tableViewModel;
        }

        #region UserControl Transition
        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.placeData.tableViewModel.SelectedTable = null;
            App.uIStateManager.SwitchCustomControl(CustomControlType.PLACE);
        }

        private void btnMoveToPay_Click(object sender, RoutedEventArgs e)
        {
            if (App.placeData.tableViewModel.SelectedTable == null)
            {
                MessageBox.Show("이용하실 테이블을 선택해 주세요!");
            }
            else
            {
                App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
            }
        }
        #endregion
    }
}
