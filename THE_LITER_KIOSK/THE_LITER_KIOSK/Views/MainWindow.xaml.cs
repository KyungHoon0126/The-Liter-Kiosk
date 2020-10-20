using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region DispatcherTimer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            #endregion

            LoadData();
            SetCustomControls();
            SetStartCustomControl();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            tbClock.Text = DateTime.Now.ToString("tt H시 mm분 ss초 dddd");
        }

        private void LoadData()
        {
            App.orderData.orderViewModel.LoadOrderData();
        }

        private void SetCustomControls()
        {
            App.uIStateManager.SetCustomCtrl(CtrlHome, CustomControlType.HOME);
            App.uIStateManager.SetCustomCtrl(CtrlTable, CustomControlType.TABLE);
            App.uIStateManager.SetCustomCtrl(CtrlOrder, CustomControlType.ORDER);
            App.uIStateManager.SetCustomCtrl(CtrlPlace, CustomControlType.PLACE);
            App.uIStateManager.SetCustomCtrl(CtrlPaySelect, CustomControlType.PAY);
            App.uIStateManager.SetCustomCtrl(CtrlPayCash, CustomControlType.PAYCASH);
            App.uIStateManager.SetCustomCtrl(CtrlPayCard, CustomControlType.PAYCARD);
            App.uIStateManager.SetCustomCtrl(CtrlPayComplete, CustomControlType.PAYCOMPLETE);
        }

        private void SetStartCustomControl()
        {
            App.uIStateManager.PushCustomCtrl(CtrlHome);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            if (CtrlOrder.lvOrderList.Items.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("주문을 취소하시겠습니까?", "Order", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MoveOrderToHome();
                        break;
                    case MessageBoxResult.No:
                        return;
                }
            }
            
            MoveOrderToHome();
        }

        private void MoveOrderToHome()
        {
            App.orderData.orderViewModel.ClearOrderedMenuDatas();
            CtrlOrder.lvOrderList.ClearValue(ItemsControl.ItemsSourceProperty);
            App.uIStateManager.SwitchCustomControl(CustomControlType.HOME);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (App.uIStateManager.customCtrlStack.Peek() == CtrlHome)
            {
                if (e.Key == Key.F2)
                {
                    MessageBox.Show("통계화면은 준비중 입니다.");
                }
            }
        }
    }
}
