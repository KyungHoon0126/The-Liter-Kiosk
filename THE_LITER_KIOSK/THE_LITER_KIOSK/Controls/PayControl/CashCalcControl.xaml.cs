using System.Windows;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.PayControl
{
    /// <summary>
    /// CashCalcControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CashCalcControl : CustomControlModel
    {
        public delegate void OnCompletePayByCashHandler();
        public event OnCompletePayByCashHandler OnCompletePayByCash;

        public CashCalcControl()
        {
            InitializeComponent();
            Loaded += CashCalcControl_Loaded;
        }

        private void CashCalcControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;
        }

        #region UserControl Transition
        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }

        private void btnCashCalcOk_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCOMPLETE);
            OnCompletePayByCash?.Invoke();
        }
        #endregion
    }
}
