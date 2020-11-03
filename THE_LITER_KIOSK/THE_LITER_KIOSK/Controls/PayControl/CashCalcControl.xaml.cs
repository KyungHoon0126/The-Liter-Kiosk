using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            tbCardNumber.Focus();
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
        #endregion

        private void CustomControlModel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                tbCardNumber.Text += e.Key;
            }
            Debug.WriteLine(tbCardNumber.Text);
            OnCompletePayByCash?.Invoke();
        }
    }
}
