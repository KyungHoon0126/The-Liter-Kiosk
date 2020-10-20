using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using THE_LITER_KIOSK.UIManager;
using TheLitter.Core.Place.Model;

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

        private void lvTableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            var selectedTable = App.placeData.tableViewModel.SelectedTable;
            selectedTable.DispatcherTimer = new DispatcherTimer();
            selectedTable.DispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            selectedTable.DispatcherTimer.Tick += DispatcherTimer_Tick;
            selectedTable.DispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var selectedTable = (TableModel)lvTableList.SelectedItem;
            selectedTable.RemainTime = $"사용시간이 {selectedTable.LeftTime}초 남았습니다.";
            selectedTable.LeftTime = selectedTable.LeftTime - 1;

            if (selectedTable.LeftTime <= 0)
            {
                (sender as DispatcherTimer).Stop();
                selectedTable.RemainTime = string.Empty;
                lvTableList.SelectedIndex = -1;
            }
        }
    }
}
