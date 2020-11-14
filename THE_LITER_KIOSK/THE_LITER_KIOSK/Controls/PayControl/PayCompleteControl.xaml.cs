using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.PayControl
{
    /// <summary>
    /// PayCompleteControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PayCompleteControl : CustomControlModel
    {
        public DispatcherTimer dispatcherTimer { get; set; }
        public int remainTime = 10;

        public PayCompleteControl()
        {
            InitializeComponent();
            Loaded += PayCompleteControl_Loaded;
        }

        private void PayCompleteControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;
            tbMemberName.DataContext = App.memberData.memberViewModel;
            tbMemberBarCode.DataContext = App.memberData.memberViewModel;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(CompletePayByCashTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void CompletePayByCashTimer_Tick(object sender, EventArgs e)
        {
            tbTimer.Text = string.Format($"{remainTime}초 후에 홈 화면으로 이동합니다.");
            remainTime--;
            if (remainTime < 0)
            {
                (sender as DispatcherTimer).Stop();
                remainTime = 10;
                App.orderData.InitData();
                App.uIStateManager.SwitchCustomControl(CustomControlType.HOME);
                App.qrIndex = 0;                
                App.placeData.tableViewModel.SelectedTable = null;
            }
        }
    }
}
