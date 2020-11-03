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
        public CardCalcControl()
        {
            InitializeComponent();
            Loaded += CardCalcControl_Loaded;
        }

        private void CardCalcControl_Loaded(object sender, RoutedEventArgs e)
        {
            webcam.CameraIndex = 0;
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
            var qrCode = App.orderData.orderViewModel.QrCode;
            if (qrCode != null)
            {
                return;
            }

            qrCode = e;

            var selectedTable = App.placeData.tableViewModel.SelectedTable;
            var memberId = App.memberData.memberViewModel.Id;
            var payTime = DateTime.Now;
            var paymentType = PaymentType.CARD.ToString();

            if (selectedTable != null)
            {
                // 매장 식사
                App.orderData.orderViewModel.SaveSalesInformation(payTime, paymentType, selectedTable.TableIdx, memberId);
                App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCOMPLETE);
            }
            else
            {
                // 포장 주문
                App.orderData.orderViewModel.SaveSalesInformation(payTime, paymentType, null, memberId);
                App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCOMPLETE); 
            }
        }
    }
}
