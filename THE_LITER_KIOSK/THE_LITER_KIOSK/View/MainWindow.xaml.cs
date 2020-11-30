using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using THE_LITER_KIOSK.Network;
using THE_LITER_KIOSK.UIManager;
using THE_LITER_KIOSK.Util;
using TheLiter.Core.Network;
using TheLiter.Core.Network.Model;

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
            Task.Run(() => LoadData());
            SetCustomControls();
            SetStartCustomControl();
            App.adminData.adminViewModel.SyncProgramOpertaionTime();
            TcpHelper.InitializeClient();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            App.adminData.adminViewModel.IncreaseOperationTime();
            tbClock.Text = DateTimeExtension.ConvertDateTimeToDayOfTheWeek(DateTime.Now);
            PollingServerConnectionState();
        }

        #region Init
        private void LoadData()
        {
            App.orderData.LoadData();
            App.placeData.LoadTableData();
            App.adminData.LoadSalesData();
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
            if (App.orderData.IsValidOrderedMenuItems())
            {
                MessageBoxResult result = MessageBox.Show("주문을 취소하시겠습니까?", "주문 화면", MessageBoxButton.YesNo);
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

        private void OnSocketLogin()
        {
            TcpHelper.IsAvailable = App.networkManager.CheckServerState();

            if (TcpHelper.IsAvailable && !TcpHelper.SocketClient.Connected)
            {
                new Thread(() =>
                {
                    #region SET LOGIN PACKET
                    TcpModel tcpPacket = new TcpModel();
                    List<MenuModel> menuItems = new List<MenuModel>();
                    tcpPacket.MessageType = (int)EMessageType.LOGIN;
                    tcpPacket.Id = App.memberData.memberViewModel.Id;
                    tcpPacket.ShopName = "";
                    tcpPacket.Content = "";
                    tcpPacket.OrderNumber = "";
                    tcpPacket.MenuItems = menuItems;
                    #endregion
                    App.networkManager.ConnectSocket(tcpPacket);
                }).Start();

                tbCurAccessTime.Text = $"최근 서버 접속 시간 : {DateTimeExtension.ConvertDateTime(DateTime.Now)}";
                tbCurAccessTime.Foreground = Brushes.Green;
            }
            else
            {
                tbCurAccessTime.Text = "서버 연결 실패";
                tbCurAccessTime.Foreground = Brushes.Red;
                TcpHelper.IsAvailable = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Stack<CustomControlModel> customControls = App.uIStateManager.customCtrlStack;
            if (e.Key == Key.F2 && customControls.Count > 0 && customControls.Peek() == CtrlHome)
            {
                Parallel.Invoke(() =>
                {
                    App.adminData.SetStatisticData();
                    App.adminData.LoadChartData();
                    App.memberData.GetAllMemberData();
                });
                App.uIStateManager.SwitchCustomControl(CustomControlType.ADMIN);
            }
        }

        private void CtrlLogin_OnLoginResultRecieved(object sender, bool success)
        {
            OnSocketLogin();

            if (success)
            {
                CtrlLogin.Visibility = Visibility.Collapsed;
                MessageBox.Show("로그인에 성공하셨습니다.");

                App.memberData.GetMemberData();
                
                if (TcpHelper.IsAvailable)
                {
                    MoveOrderToHome();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("서버 연결이 안된채로 계속 하시겠습니까?", "서버 연결", MessageBoxButton.YesNo);
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
            App.networkManager.DisconnectSocket();
        }

        private void btnRedirectSocket_Click(object sender, RoutedEventArgs e)
        {
            if (!TcpHelper.SocketClient.Connected)
            {
                OnSocketLogin();
            }
            else
            {
                MessageBox.Show("이미 서버에 연결되어 있습니다.");
            }
        }

        private void PollingServerConnectionState()
        {
            try
            {
                if (TcpHelper.SocketClient.Poll(0, SelectMode.SelectRead))
                {
                    App.networkManager.DisconnectSocket();
                    tbCurAccessTime.Text = "서버와의 연결이 끊어졌습니다.";
                    tbCurAccessTime.Foreground = Brushes.Red;
                }
            }
            catch (SocketException e)
            {
                Debug.WriteLine($"CHECK SERVER CONNECTION STATE ERROR : {e.Message}");
            }
        }
    }
}
