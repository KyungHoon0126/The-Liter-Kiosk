using System;
using System.Windows;
using System.Windows.Input;
using THE_LITER_KIOSK.DataBase.Models;
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
            tbCardNumber.Focus(); // TODO : Focus가 안됨.
            Loaded += CashCalcControl_Loaded;
        }

        private void CashCalcControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.orderData.orderViewModel;
        }

        // TODO : BarCode 값이 읽어졌는데, 다시 해보니까 안됨.
        private void CustomControlModel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                App.orderData.orderViewModel.BarCode = tbCardNumber.Text;
            }

            if (App.memberData.memberViewModel.BarCode == App.orderData.orderViewModel.BarCode)
            {
                App.orderData.orderViewModel.SaveSalesInformation(DateTime.Now, PaymentType.CASH.ToString(), null, App.memberData.memberViewModel.Id);
                App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCOMPLETE);
                OnCompletePayByCash?.Invoke();
            }
            else
            {
                MessageBox.Show("결제 정보가 일치하지 않습니다");
                tbCardNumber.Text = string.Empty;
                return;
            }
        }

        #region UserControl Transition
        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }
        #endregion
    }
}
