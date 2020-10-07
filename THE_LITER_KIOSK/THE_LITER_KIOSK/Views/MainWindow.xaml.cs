using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using THE_LITER_KIOSK.Controls.HomeControl;
using THE_LITER_KIOSK.Controls.OrderControl;
using THE_LITER_KIOSK.Controls.PayControl;
using THE_LITER_KIOSK.Controls.PlaceControl;
using THE_LITER_KIOSK.Controls.TableControl;

namespace THE_LITER_KIOSK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer;
        Stack<UserControl> controls;
        
        #region Controls
        HomeControl homeControl = new HomeControl();
        OrderControl orderControl = new OrderControl();
        PlaceControl placeControl = new PlaceControl();
        TableControl tableControl = new TableControl();
        PayControl payControl = new PayControl();
        #endregion

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

            CtrlHome.Visibility = Visibility.Visible;

            // #1
            CtrlHome.btnOrder.Click += BtnOrder_Click; // 홈 -> 주문
            CtrlOrder.OnLoadPlaceControl += CtrlOrder_OnLoadPlaceControl; // 주문 -> 장소 선택

            // #2
            CtrlPlace.btnStoreMeal.Click += BtnStoreMeal_Click; // 장소 -> 테이블 (매장 식사)
            CtrlPlace.btnPackingMeal.Click += BtnPackingMeal_Click; // 장소 -> 테이블 (포장 주문)
            CtrlPlace.btnPrev.Click += BtnPrev_Click; // 장소 -> 이전(주문)

            // #3
            CtrlTable.btnTablePrev.Click += BtnTablePrev_Click; // 테이블 -> 장소
            CtrlTable.btnMoveToPay.Click += BtnMoveToPay_Click; // 테이블 -> 다음(결제)
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            tbClock.Text = DateTime.Now.ToString("tt H시 mm분 ss초 dddd");
        }

        private void InitData()
        {
            App.orderData.orderViewModel.LoadData();
            controls = new Stack<UserControl>();
        }

        // TODO : 페이지 관리 구현하기.
        private void SetControlsStack()
        {
            //controls.Push(CtrlHome);
            //controls.Push(CtrlOrder);
            //controls.Push(CtrlPlace);
            //controls.Push(CtrlTable);
            //controls.Push(CtrlPay);

            controls.Push(homeControl);
            controls.Push(orderControl);
            controls.Push(placeControl);
            controls.Push(tableControl);
            controls.Push(payControl);
        }

        #region Pagle Transform
        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            CtrlHome.Visibility = Visibility.Collapsed;
            gdMain.Visibility = Visibility.Visible;
            CtrlOrder.Visibility = Visibility.Visible;
        }

        private void CtrlOrder_OnLoadPlaceControl(object sender, EventArgs e)
        {
            CtrlOrder.Visibility = Visibility.Collapsed;
            CtrlPlace.Visibility = Visibility.Visible;
        }

        private void BtnStoreMeal_Click(object sender, RoutedEventArgs e)
        {
            CtrlPlace.Visibility = Visibility.Collapsed;
            CtrlTable.Visibility = Visibility.Visible;
        }

        private void BtnPackingMeal_Click(object sender, RoutedEventArgs e)
        {
            CtrlTable.Visibility = Visibility.Collapsed;
            CtrlPay.Visibility = Visibility.Visible;
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            CtrlPlace.Visibility = Visibility.Collapsed;
            CtrlOrder.Visibility = Visibility.Visible;
        }

        private void BtnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            CtrlTable.Visibility = Visibility.Collapsed;
            CtrlPlace.Visibility = Visibility.Visible;
        }

        private void BtnMoveToPay_Click(object sender, RoutedEventArgs e)
        {
            CtrlTable.Visibility = Visibility.Collapsed;
            CtrlPay.Visibility = Visibility.Visible;
        }
        #endregion

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            if (CtrlOrder.lvOrderList.Items.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("주문을 취소하시겠습니까?", "Order", MessageBoxButton.YesNoCancel);
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
            else
            {
                MoveOrderToHome();
            }
        }

        private void MoveOrderToHome()
        {
            gdMain.Visibility = Visibility.Collapsed;
            CtrlOrder.Visibility = Visibility.Collapsed;
            CtrlPlace.Visibility = Visibility.Collapsed;
            CtrlTable.Visibility = Visibility.Collapsed;
            CtrlHome.Visibility = Visibility.Visible;
        }
    }
}
