using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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
            //App.placeData.LoadTableData();
            this.DataContext = App.placeData.tableViewModel;
        }

        #region UserControl Transition
        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PLACE);
        }

        private void btnMoveToPay_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }
        #endregion

        private void lvTableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            var selectedTable = App.placeData.tableViewModel.SelectedTable;
            selectedTable.DispatcherTimer = new DispatcherTimer();
            selectedTable.DispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            selectedTable.DispatcherTimer.Tick += selectedTable.DispatcherTimer_Tick;
            selectedTable.DispatcherTimer.Start();
            if(selectedTable.IsUsed)
            {
                MessageBox.Show("이미 사용중인 테이블 입니다.");
                
                return;
            }
            else
            {
                selectedTable.IsUsed = true;

            }
        }
    }
}
