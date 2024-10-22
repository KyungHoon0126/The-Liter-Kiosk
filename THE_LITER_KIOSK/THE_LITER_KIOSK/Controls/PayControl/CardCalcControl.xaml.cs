﻿using System;
using System.Threading.Tasks;
using System.Windows;
using THE_LITER_KIOSK.DataBase.Models;
using THE_LITER_KIOSK.Network;
using THE_LITER_KIOSK.UIManager;

namespace THE_LITER_KIOSK.Controls.PayControl
{
    /// <summary>
    /// CardCalcControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CardCalcControl : CustomControlModel
    {
        public delegate void OnCompletePayByCardHandler();
        public event OnCompletePayByCardHandler OnCompletePayByCard;

        delegate void SetPayByCardDelegate(int? tableIdx);

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

        private void webcam_QrDecoded(object sender, string e)
        {
            if (App.qrIndex >= 1) return;

            App.orderData.orderViewModel.QrCode = e;

            if (App.memberData.memberViewModel.QrCode == e)
            {
                var selectedTable = App.placeData.tableViewModel.SelectedTable;
                var payTime = DateTime.Now;
                var memberId = App.memberData.memberViewModel.Id;

                SetPayByCardDelegate setMethod = (tableIdx) =>
                {
                    ClearQrCode();
                    SaveSalesInformation(payTime, PaymentType.CARD.ToString(), tableIdx, memberId);

                    var item = App.orderData.orderViewModel.ReceiptIdx;

                    if (TcpHelper.SocketClient.Connected)
                    {
                        App.networkManager.Send(TcpHelper.SocketClient, App.networkManager.SetOrderMsgArgs(App.orderData.orderViewModel.SendOrderInfoToNormal(memberId)));
                        App.networkManager.Send(TcpHelper.SocketClient, App.networkManager.SetGroupMsgArgs(App.orderData.orderViewModel.SendOrderInfoToGroup(memberId, App.orderData.orderViewModel.OrderedMenuItems)));
                    }
                    
                    App.qrIndex++;
                    App.uIStateManager.SwitchCustomControl(CustomControlType.PAYCOMPLETE);
                    OnCompletePayByCard?.Invoke();
                };

                if (selectedTable != null)
                {
                    selectedTable.PayTime = payTime.ToString();
                    setMethod(selectedTable.TableIdx);
                }
                else
                {
                    setMethod(null);
                }
            }
            else
            {
                MessageBox.Show("결제 정보가 일치하지 않습니다");
                tbRecog.Text = string.Empty;
                ClearQrCode();
                qcWebcam.CameraIndex = 0;
            }
        }

        private void SaveSalesInformation(DateTime payTime, string paymentType, int? tableIdx, string memberId)
        {
            Task.Run(() => App.orderData.orderViewModel.SaveSalesInformation(payTime, paymentType, tableIdx, memberId));
        }

        private void ClearQrCode()
        {
            App.orderData.orderViewModel.ClearQrCode();
        }

        #region UserControl Transition
        private void btnTablePrev_Click(object sender, RoutedEventArgs e)
        {
            App.uIStateManager.SwitchCustomControl(CustomControlType.PAY);
        }
        #endregion
    }
}
