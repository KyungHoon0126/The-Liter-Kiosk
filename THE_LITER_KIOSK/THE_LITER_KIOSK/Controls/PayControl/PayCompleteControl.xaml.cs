using System;
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
        public DispatcherTimer payCompleteTimer { get; set; }
        public int remainTime = 5;

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

            payCompleteTimer = new DispatcherTimer();
            payCompleteTimer.Tick += CompletePayTimer_Tick;
            payCompleteTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void CompletePayTimer_Tick(object sender, EventArgs e)
        {
            tbTimer.Text = string.Format($"{remainTime}초 후에 홈 화면으로 이동합니다.");
            remainTime--;
            if (remainTime < 0)
            {
                (sender as DispatcherTimer).Stop();
                remainTime = 5;
                App.orderData.InitData();
                App.qrIndex = 0;
                App.placeData.tableViewModel.SelectedTable = null;
                App.uIStateManager.SwitchCustomControl(CustomControlType.HOME);             
            }
        }
    }
}
