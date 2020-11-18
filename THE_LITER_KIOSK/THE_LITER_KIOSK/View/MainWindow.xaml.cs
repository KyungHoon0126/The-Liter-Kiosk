using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using THE_LITER_KIOSK.Network;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer;

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        #endregion

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

            App.adminData.adminViewModel.SynchronizationOpertaionTime();
            
            TcpHelper.InitializeClient();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            App.adminData.adminViewModel.IncreaseOperationTime();
            tbClock.Text = DateTime.Now.ToString("tt H시 mm분 ss초 dddd");
        }

        #region Init
        private void LoadData()
        {
            App.orderData.LoadData();
            App.placeData.LoadTableData();
            App.adminData.adminViewModel.GetAllSalesInformation();
            App.orderData.SetPagingMenuItems();
        }

        private void SetCustomControls()
        {
            App.uIStateManager.SetCustomCtrl(CtrlAdmin, CustomControlType.ADMIN);
            App.uIStateManager.SetCustomCtrl(CtrlHome, CustomControlType.HOME);
            App.uIStateManager.SetCustomCtrl(CtrlTable, CustomControlType.TABLE);
            App.uIStateManager.SetCustomCtrl(CtrlOrder, CustomControlType.ORDER);
            App.uIStateManager.SetCustomCtrl(CtrlPlace, CustomControlType.PLACE);
            App.uIStateManager.SetCustomCtrl(CtrlPaySelect, CustomControlType.PAY);
            App.uIStateManager.SetCustomCtrl(CtrlPayCash, CustomControlType.PAYCASH);
            App.uIStateManager.SetCustomCtrl(CtrlPayCard, CustomControlType.PAYCARD);
            App.uIStateManager.SetCustomCtrl(CtrlPayComplete, CustomControlType.PAYCOMPLETE);
            App.uIStateManager.SetCustomCtrl(CtrlLogin, CustomControlType.LOGIN);
            App.uIStateManager.SetCustomCtrl(CtrlSignup, CustomControlType.SIGNUP);
        }

        private void SetStartCustomControl()
        {
            App.uIStateManager.PushCustomCtrl(CtrlLogin);
        }
        #endregion

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            if (App.orderData.orderViewModel.IsOrderedMenuItemsValid())
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
            App.orderData.InitData();
            App.uIStateManager.SwitchCustomControl(CustomControlType.HOME);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (App.uIStateManager.customCtrlStack.Count > 0 && e.Key == Key.F2 && App.uIStateManager.customCtrlStack.Peek() == CtrlHome)
            {
                App.adminData.LoadData();
                App.memberData.GetAllMemberData();
                App.uIStateManager.SwitchCustomControl(CustomControlType.ADMIN);
            }
        }

        private void CtrlLogin_OnLoginResultRecieved(object sender, bool success)
        {
            if (success)
            {
                CtrlLogin.Visibility = Visibility.Collapsed;
                App.memberData.GetMemberData();

#if false
                if (App.tcpClient.CheckServerState())
                // var isConnected = App.tcpClient.CheckServerState();
#endif

#if true
                if (true)
#endif
                {
                    App.uIStateManager.SwitchCustomControl(CustomControlType.HOME);

                    // 경훈: Socket이 계속 연결되어 있어야 하므로, 새로운 Thread로 처리.
                    new Thread(() =>
                    {
                        TcpModel tcpModel = new TcpModel();
                        List<MenuModel> menuItems = new List<MenuModel>();
                        tcpModel.MessageType = 0;
                        //tcpModel.Id = App.memberData.memberViewModel.Id;
                        tcpModel.Id = "2107";
                        tcpModel.ShopName = "";
                        tcpModel.Content = "";
                        tcpModel.OrderNumber = "";
                        tcpModel.MenuItems = menuItems;

                        App.tcpClient.StartClient(tcpModel);
                    }).Start();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("서버 연결이 안된채로 수행하시겠습니까?", "Login", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            MoveOrderToHome();
                            break;
                        case MessageBoxResult.No:
                            App.uIStateManager.SwitchCustomControl(CustomControlType.LOGIN);
                            break;
                    }
                }
            }
        }

        private void CtrlPay_OnCompletePay()
        {
            CtrlPayComplete.payCompleteTimer.Start();
            App.placeData.tableViewModel.RunPayCompleteTimer();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.adminData.adminViewModel.SaveProgramTotalUsageTime();
        }
    }
}
