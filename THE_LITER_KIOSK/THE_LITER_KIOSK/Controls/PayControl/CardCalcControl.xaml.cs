using System;
using System.Windows;
using THE_LITER_KIOSK.DataBase.Models;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.PayControl
{
    /// <summary>
    /// CardCalcControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CardCalcControl : CustomControlModel
    {
        public delegate void OnCompletePayByCashHandler();
        public event OnCompletePayByCashHandler OnCompletePayByCash;

        public CardCalcControl()
        {
            InitializeComponent();
            Loaded += CardCalcControl_Loaded;
        }

        private void CardCalcControl_Loaded(object sender, RoutedEventArgs e)
        {
            qcWebcam.CameraIndex = 0;
            this.DataContext = App.orderData.orderViewModel;
        }

        #region UserControl Transition
        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }
        #endregion

        private void webcam_QrDecoded(object sender, string e)
        {
            if (App.orderData.orderViewModel.QrCode != null)
            {
                return;
            }
            
            App.orderData.orderViewModel.QrCode = e;

            if (App.memberData.memberViewModel.QrCode == App.orderData.orderViewModel.QrCode)
            {
                var selectedTable = App.placeData.tableViewModel.SelectedTable;
                var memberId = App.memberData.memberViewModel.Id;
                var payTime = DateTime.Now;
                var paymentType = PaymentType.CARD.ToString();

                if (selectedTable != null)
                {
                    App.orderData.orderViewModel.SaveSalesInformation(payTime, paymentType, selectedTable.TableIdx, memberId);
                    App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCOMPLETE);
                    OnCompletePayByCash?.Invoke();
                }
                else
                {
                    App.orderData.orderViewModel.SaveSalesInformation(payTime, paymentType, null, memberId);
                    App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCOMPLETE);
                    OnCompletePayByCash?.Invoke();
                }
            }
            else
            {
                MessageBox.Show("결제 정보가 일치하지 않습니다");
                tbRecog.Text = string.Empty;
                App.orderData.orderViewModel.QrCode = null;
                qcWebcam.CameraIndex = 0;
                return;
            }
        }
    }
}
