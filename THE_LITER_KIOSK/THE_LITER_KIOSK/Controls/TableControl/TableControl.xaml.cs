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
        // DispatcherTimer timer = new DispatcherTimer();
        int leftTime = 60;
        DispatcherTimer tempTimer = new DispatcherTimer();

        public TableControl()
        {
            InitializeComponent();
            Loaded += TableControl_Loaded;

        }

        private async void TableControl_Loaded(object sender, RoutedEventArgs e)
        {
            await App.placeData.tableViewModel.LoadTableData();
            this.DataContext = App.placeData.tableViewModel;
           // timer.Interval = TimeSpan.FromSeconds(1);
           // timer.Tick += Timer_Tick;
        }

        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PLACE);
        }

        private void btnMoveToPay_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }

        private void lvTableList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // timer.Start();
            TableModel selectTable = (TableModel)lvTableList.SelectedItem;
            tempTimer = new DispatcherTimer(); 
            tempTimer = selectTable.DispatcherTimer;
            /* selectTable.DispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            selectTable.DispatcherTimer.Tick += DispatcherTimer_Tick;
            selectTable.DispatcherTimer.Start(); */
            tempTimer.Interval = TimeSpan.FromSeconds(1);
            tempTimer.Tick += TempTimer_Tick;
            tempTimer.Start();
        }

        private void TempTimer_Tick(object sender, EventArgs e)
        {
           var tableItems = App.placeData.tableViewModel.TableItems;
            for(int i = 0; i < tableItems.Count; i += 1)
            {
                if (tableItems[i].TableIdx == ((TableModel)lvTableList.SelectedItem).TableIdx)
                {
                    tableItems[i].RemainTime = $"사용시간이 {leftTime}초 남았습니다.";
                    leftTime -= 1;
                    if(leftTime <= 0)
                    {
                        tempTimer.Stop();
                    }
                }
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
           var tableItems = App.placeData.tableViewModel.TableItems;
            for(int i = 0; i < tableItems.Count; i += 1)
            {
                if (tableItems[i].TableIdx == ((TableModel)lvTableList.SelectedItem).TableIdx)
                {
                    tableItems[i].RemainTime = $"사용시간이 {leftTime}초 남았습니다.";
                    leftTime -= 1;
                    if(leftTime <= 0)
                    {
                                  
                    }
                }
            }
        }

       /* private void Timer_Tick(object sender, EventArgs e)
        {
           var tableItems = App.placeData.tableViewModel.TableItems;
            for(int i = 0; i < tableItems.Count; i += 1)
            {
                if (tableItems[i].TableIdx == ((TableModel)lvTableList.SelectedItem).TableIdx)
                {
                    tableItems[i].RemainTime = $"사용시간이 {leftTime}초 남았습니다.";
                    leftTime -= 1;
                    if(leftTime <= 0)
                    {
                        timer.Stop();
                    }
                }
            }
            //RemainTime = leftTime.ToString();

        }*/

    }
}
