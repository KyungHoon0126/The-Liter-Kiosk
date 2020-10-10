using System;
using System.Windows;
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

            InitData();
            SetControlsStack();
            SetStartControl();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            tbClock.Text = DateTime.Now.ToString("tt H시 mm분 ss초 dddd");
        }

        private void InitData()
        {
            App.orderData.orderViewModel.LoadData();
        }

        private void SetControlsStack()
        {
            App.uIStateManager.SetCustomCtrl(CtrlHome, CustomControlType.HOME);
            App.uIStateManager.SetCustomCtrl(CtrlTable, CustomControlType.TABLE);
            App.uIStateManager.SetCustomCtrl(CtrlOrder, CustomControlType.ORDER);
            App.uIStateManager.SetCustomCtrl(CtrlPlace, CustomControlType.PLACE);
            App.uIStateManager.SetCustomCtrl(CtrlPay, CustomControlType.PAY);
        }

        private void SetStartControl()
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
                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            
            MoveOrderToHome();
        }

        private void MoveOrderToHome()
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.HOME);
        }
    }
}
